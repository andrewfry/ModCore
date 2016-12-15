﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Routing;
using ModCore.Abstraction.DataAccess;
using ModCore.Abstraction.Plugins;
using ModCore.Abstraction.Services.Access;
using ModCore.Abstraction.Site;
using ModCore.Core.HelperExtensions;
using ModCore.Models.Access;
using ModCore.Models.Enum;
using ModCore.Services.Base;
using ModCore.Specifications.Access;
using ModCore.Utilities.HelperExtensions;
using RoleBasedPermisison.Abstraction;
using RoleBasedPermisison.Plugin.Models;
using RoleBasedPermission.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace RoleBasedPermisison.Plugin.Services
{
    public class PermissionService : BaseServiceAsync<Permission>, IPermissionService, IPermissionManagerService
    {

        private readonly IActionDescriptorCollectionProvider _actionDescriptorProvider;
        private List<Permission> _currentPermissions;
        private PermissonCache _permissonCache;

        public List<Permission> CurrentPermissions
        {
            get
            {
                if (_currentPermissions == null)
                {
                    _currentPermissions = GetCurrentPermissions().Result;
                }

                return _currentPermissions;
            }
        }

        public PermissionService(IDataRepositoryAsync<Permission> repos, IMapper mapper, ILog logger, IActionDescriptorCollectionProvider actionDescriptorProvider) :
            base(repos, mapper, logger)
        {
            _actionDescriptorProvider = actionDescriptorProvider;
            _permissonCache = new PermissonCache();
        }

        public List<PermissionDiscriptor> GetControllerDiscriptor()
        {
            var currentDescriptors = _actionDescriptorProvider.ActionDescriptors.Items;
            var assemblyPermission = new List<PermissionDiscriptor>();

            foreach (var desc in currentDescriptors)
            {
                var controllerDesc = desc as ControllerActionDescriptor;
                if (controllerDesc != null)
                {
                    var assembly = controllerDesc.ControllerTypeInfo.Assembly;
                    var assemblyName = assembly.FullName;
                    IPlugin pluginInstance = null;
                    var pluginType = assembly.GetImplementationOrDefault<IPlugin>();
                    if (pluginType != null)
                    {
                        pluginInstance = assembly.GetInstance<IPlugin>();
                    }

                    var assemblyIsPlugin = pluginType != null;
                    var controllerName = controllerDesc.ControllerName;
                    var actionName = controllerDesc.ActionName;
                    var areaName = controllerDesc.RouteValues["area"] == null ? "" : controllerDesc.RouteValues["area"].ToUpper();

                    var assemblyPerm = assemblyPermission.SingleOrDefault(a => a.AssemblyName == assemblyName);
                    if (assemblyPerm == null)
                    {
                        assemblyPerm = new PermissionDiscriptor
                        {
                            AssemblyName = assemblyName,
                            Name = assemblyIsPlugin ? pluginInstance.Name : "ModCore Www",
                            IsPlugin = assemblyIsPlugin,
                            Description = assemblyIsPlugin ? pluginInstance.Description : "Main ModCore Application",
                        };

                        assemblyPermission.Add(assemblyPerm);
                    }


                    var areaPerm = assemblyPerm.AreaPermissons.SingleOrDefault(a => a.Name.ToUpper() == areaName.ToUpper());
                    if (areaPerm == null)
                    {
                        areaPerm = new PermissionAreaDiscriptor
                        {
                            Name = areaName
                        };

                        assemblyPerm.AreaPermissons.Add(areaPerm);
                    }

                    var controllerPerm = areaPerm.ControllerPermissons.SingleOrDefault(a => a.Name.ToUpper() == controllerName.ToUpper());
                    if (controllerPerm == null)
                    {
                        controllerPerm = new PermissionControllerDiscriptor
                        {
                            Name = controllerName
                        };

                        areaPerm.ControllerPermissons.Add(controllerPerm);
                    }

                    var attributes = controllerDesc.MethodInfo.CustomAttributes.ToList();
                    var methodType = attributes.Any(a => a.AttributeType.GetType() == typeof(HttpGetAttribute)) ? HttpMethod.Get
                                            : attributes.Any(a => a.AttributeType.GetType() == typeof(HttpPostAttribute)) ? HttpMethod.Post
                                            : attributes.Any(a => a.AttributeType.GetType() == typeof(HttpPutAttribute)) ? HttpMethod.Put
                                            : attributes.Any(a => a.AttributeType.GetType() == typeof(HttpDeleteAttribute)) ? HttpMethod.Delete
                                            : HttpMethod.Get;

                    var actionPerm = controllerPerm.ActionPermissons.SingleOrDefault(a => a.Name.ToUpper() == actionName.ToUpper() && a.Method == methodType);
                    if (actionPerm == null)
                    {
                        actionPerm = new PermissionActionDiscriptor
                        {
                            Name = actionName,
                            Method = methodType
                        };

                        controllerPerm.ActionPermissons.Add(actionPerm);
                    }

                }
            }

            return assemblyPermission;
        }

        public PermissionResult CheckPermission(Controller controller, ActionDescriptor action, User user)
        {
            throw new NotImplementedException();
        }

        public PermissionResult CheckPermission(Controller controller, ActionDescriptor action, List<string> userRoleIds)
        {
            var assemblyName = controller.ControllerContext.ActionDescriptor.ControllerTypeInfo.Assembly.FullName.ToUpper();
            var actionName = action.DisplayName.ToUpper();
            var controllerName = controller.ControllerContext.ActionDescriptor.ControllerName.ToUpper();
            var areaName = action.RouteValues["area"] == null ? "" : action.RouteValues["area"].ToUpper();

            PermissionResult result = PermissionResult.NotDefined();

            foreach (var roleId in userRoleIds)
            {
                var cachedResult = _permissonCache.FromCache(assemblyName, areaName, controllerName, actionName, roleId);
                if (cachedResult.HasValue && (cachedResult.Value == PermissionExecutedResult.Denied ||
                    cachedResult.Value == PermissionExecutedResult.Granted))
                {
                    return PermissionResult.FromEnum(cachedResult.Value);
                }
                else if (!cachedResult.HasValue)
                {
                    var dbCheck = GetCheckedPermission(controller, action, roleId);
                    if (dbCheck.ErrorOccured)
                        return dbCheck;

                    _permissonCache.AddToCache(assemblyName, areaName, controllerName, actionName, roleId, dbCheck.PermissionExecutedResult);

                    if (dbCheck.PermissionExecutedResult == PermissionExecutedResult.Denied ||
                            dbCheck.PermissionExecutedResult == PermissionExecutedResult.Granted)
                    {
                        return dbCheck;
                    }
                }
            }

            return result;
        }

        private PermissionResult GetCheckedPermission(Controller controller, ActionDescriptor action, string roleId)
        {
            var assemblyName = controller.ControllerContext.ActionDescriptor.ControllerTypeInfo.Assembly.FullName.ToUpper();
            var actionName = action.DisplayName.ToUpper();
            var controllerName = controller.ControllerContext.ActionDescriptor.ControllerName.ToUpper();
            var areaName = action.RouteValues["area"] == null ? "" : action.RouteValues["area"].ToUpper();

            var assemblyPermission = CurrentPermissions.Where(a => a is PermissionAssembly)
                .Select(a => a as PermissionAssembly)
                .FirstOrDefault(a => a.AssemblyName == assemblyName);

            var result = GetPermissionResult(assemblyPermission, roleId);
            if (result == PermissionExecutedResult.Granted)
                return PermissionResult.Granted();
            else if (result == PermissionExecutedResult.Denied)
                return PermissionResult.Denied();

            foreach (var area in assemblyPermission.AreaPermissions)
            {
                var resultArea = GetPermissionResult(area, roleId);
                if (resultArea == PermissionExecutedResult.Granted)
                    return PermissionResult.Granted();
                else if (resultArea == PermissionExecutedResult.Denied)
                    return PermissionResult.Denied();


                foreach (var controllerPerm in area.ControllerPermissons)
                {
                    var resultController = GetPermissionResult(controllerPerm, roleId);
                    if (resultController == PermissionExecutedResult.Granted)
                        return PermissionResult.Granted();
                    else if (resultController == PermissionExecutedResult.Denied)
                        return PermissionResult.Denied();

                    foreach (var actionPerm in controllerPerm.ActionPermissons)
                    {
                        var resultAction = GetPermissionResult(actionPerm, roleId);
                        if (resultAction == PermissionExecutedResult.Granted)
                            return PermissionResult.Granted();
                        else if (resultAction == PermissionExecutedResult.Denied)
                            return PermissionResult.Denied();
                    }
                }
            }

            return PermissionResult.NotDefined();
        }

        private PermissionExecutedResult GetPermissionResult(Permission permission, string roleId)
        {
            if (permission.DeniedRoles.Any(a => a.Id == roleId))
                return PermissionExecutedResult.Denied;

            if (permission.GrantedRoles.Any(a => a.Id == roleId))
                return PermissionExecutedResult.Granted;

            return PermissionExecutedResult.NotDefined;
        }

        private async Task<List<Permission>> GetCurrentPermissions()
        {
            var returnVal = await _repository.FindAllAsync(new GetAllPermissions());

            return returnVal.ToList();
        }
    }
}
