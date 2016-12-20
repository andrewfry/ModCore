using AutoMapper;
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
using RoleBasedPermission.Plugin.Abstraction;
using RoleBasedPermission.Plugin.Models;
using RoleBasedPermission.Plugin.Specifications;
using RoleBasedPermission.Plugin.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace RoleBasedPermission.Plugin.Services
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

        public PermissonCache Cache
        {
            get
            {
                if (this._permissonCache == null)
                    this._permissonCache = new PermissonCache();
                return this._permissonCache;
            }
        }

        public PermissionService(IDataRepositoryAsync<Permission> repos, IMapper mapper, ILog logger, IActionDescriptorCollectionProvider actionDescriptorProvider) :
            base(repos, mapper, logger)
        {
            _actionDescriptorProvider = actionDescriptorProvider;
            _permissonCache = null;
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
                var cachedResult = Cache.FromCache(assemblyName, areaName, controllerName, actionName, roleId);
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

                    Cache.AddToCache(assemblyName, areaName, controllerName, actionName, roleId, dbCheck.PermissionExecutedResult);

                    if (dbCheck.PermissionExecutedResult == PermissionExecutedResult.Denied ||
                            dbCheck.PermissionExecutedResult == PermissionExecutedResult.Granted)
                    {
                        return dbCheck;
                    }
                }
            }

            return result;
        }

        public List<vPermissionDiscriptor> GetDiscriptorsForRole(string roleId)
        {
            var assemblyPermissions = CurrentPermissions.Where(a => a is PermissionAssembly).Select(a => a as PermissionAssembly).ToList();
            var vmPermisisons = _mapper.Map<List<vPermissionDiscriptor>>(assemblyPermissions);

            foreach (var ap in assemblyPermissions)
            {
                var apVm = vmPermisisons.Single(a => a.AssemblyName == ap.AssemblyName);
                apVm.AccessGranted = ap.GrantedRoles.Any(a => a == roleId);

                foreach (var areaP in ap.AreaPermissions)
                {
                    var areaVm = apVm.AreaPermissons.Single(a => a.Name == areaP.AreaName);
                    areaVm.AccessGranted = areaP.GrantedRoles.Any(a => a == roleId);

                    foreach (var contP in areaP.ControllerPermissons)
                    {
                        var contVm = areaVm.ControllerPermissons.Single(a => a.Name == contP.ControllerName);
                        contVm.AccessGranted = contP.GrantedRoles.Any(a => a == roleId);

                        foreach (var actionP in contP.ActionPermissons)
                        {
                            var actionVm = contVm.ActionPermissons.Single(a => a.Name == actionP.ActionName
                                            && a.Method == actionP.Method);
                            actionVm.AccessGranted = actionP.GrantedRoles.Any(a => a == roleId);
                        }
                    }
                }
            }

            return vmPermisisons;
        }

        public void UpdateDiscriptors(List<vPermissionDiscriptor> discriptors, string roleId)
        {
            var permToUpdate = CurrentPermissions.ToList();
            var toUpdate = permToUpdate.Where(a => a is PermissionAssembly).Select(a => a as PermissionAssembly).ToList();

            foreach (var vmAssembly in discriptors)
            {
                var p = toUpdate.Single(a => a.AssemblyName == vmAssembly.AssemblyName);

                if (vmAssembly.AccessGranted)
                    p.GrantedRoles.Add(roleId);

                foreach(var vmArea in vmAssembly.AreaPermissons)
                {
                    var pArea = p.AreaPermissions.Single(a => a.AreaName == vmArea.Name);

                    if (vmArea.AccessGranted)
                        pArea.GrantedRoles.Add(roleId);

                    foreach(var vmCont in vmArea.ControllerPermissons)
                    {
                        var pCont = pArea.ControllerPermissons.Single(a => a.ControllerName == vmCont.Name);

                        if (vmCont.AccessGranted)
                            pCont.GrantedRoles.Add(roleId);

                        foreach (var vmAction in vmCont.ActionPermissons)
                        {
                            var pAction = pCont.ActionPermissons.Single(a => a.ActionName == vmAction.Name &&
                                            a.Method == vmAction.Method);

                            if (vmAction.AccessGranted)
                                pAction.GrantedRoles.Add(roleId);
                        }
                    }
                }
            }

            _repository.UpdateAsync(permToUpdate);
            _currentPermissions = null;
            _permissonCache = null;
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
            if (permission.DeniedRoles.Any(a => a == roleId))
                return PermissionExecutedResult.Denied;

            if (permission.GrantedRoles.Any(a => a == roleId))
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
