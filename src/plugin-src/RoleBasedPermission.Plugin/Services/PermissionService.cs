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
    public class PermissionService : BaseServiceAsync<PermissionAssembly>, IPermissionService, IPermissionManagerService
    {

        private readonly IActionDescriptorCollectionProvider _actionDescriptorProvider;
        private List<PermissionAssembly> _currentPermissions;
        private PermissonCache _permissonCache;
        private List<PermissionDiscriptor> _availablePermissions;

        public List<PermissionAssembly> CurrentPermissions
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

        public List<PermissionDiscriptor> AvailablePermissions
        {
            get
            {
                if (_availablePermissions == null)
                {
                    _availablePermissions = GetControllerDiscriptor();
                }

                return _availablePermissions;
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

        public PermissionService(IDataRepositoryAsync<PermissionAssembly> repos, IMapper mapper, ILog logger, IActionDescriptorCollectionProvider actionDescriptorProvider) :
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
                    var areaName = controllerDesc.RouteValues["area"] == null ? "" : controllerDesc.RouteValues["area"];

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
            var vmPermissions = _mapper.Map<List<vPermissionDiscriptor>>(AvailablePermissions);
            var assemblyPermissions = CurrentPermissions.Where(a => a is PermissionAssembly).Select(a => a as PermissionAssembly).ToList();

            foreach (var ap in assemblyPermissions)
            {
                var apVm = vmPermissions.Single(a => a.AssemblyName == ap.AssemblyName);
                apVm.AccessGranted = ap.GrantedRoles.Any(a => a == roleId);
                apVm.Id = ap.Id;

                foreach (var areaP in ap.AreaPermissions)
                {
                    areaP.AreaName = areaP.AreaName == null ? "" : areaP.AreaName;
                    var areaVm = apVm.AreaPermissons.Single(a => a.Name.ToUpper() == areaP.AreaName.ToUpper());
                    areaVm.AccessGranted = areaP.GrantedRoles.Any(a => a == roleId) || apVm.AccessGranted;
                    areaVm.Id = areaP.Id;

                    foreach (var contP in areaP.ControllerPermissons)
                    {
                        var contVm = areaVm.ControllerPermissons.Single(a => a.Name.ToUpper() == contP.ControllerName.ToUpper());
                        contVm.AccessGranted = contP.GrantedRoles.Any(a => a == roleId) || areaVm.AccessGranted;
                        contVm.Id = contVm.Id;

                        foreach (var actionP in contP.ActionPermissons)
                        {
                            var actionVm = contVm.ActionPermissons.Single(a => a.Name.ToUpper() == actionP.ActionName.ToUpper()
                                            && a.Method == actionP.Method);
                            actionVm.AccessGranted = actionP.GrantedRoles.Any(a => a == roleId) || contVm.AccessGranted;
                            actionVm.Id = actionVm.Id;
                        }
                    }
                }
            }

            return vmPermissions;
        }

        public async Task UpdateDiscriptorsAsync(List<vPermissionDiscriptor> discriptors, string roleId)
        {
            var toUpdate = CurrentPermissions.ToList();

            foreach (var vmAssembly in discriptors)
            {
                var p = toUpdate.SingleOrDefault(a => a.AssemblyName.ToUpper() == vmAssembly.AssemblyName.ToUpper());
                if (p == null)
                {
                    //if(!vmAssembly.AccessGranted && !vmAssembly.AnyChildrenEnabled)
                    //{
                    //    continue;
                    //}

                    p = new PermissionAssembly();
                    p.AssemblyName = vmAssembly.AssemblyName;

                    await _repository.InsertAsync(p);
                    toUpdate.Add(p);
                }

                if (vmAssembly.AccessGranted && vmAssembly.AllChildrenEnabled)
                {
                    p.GrantedRoles.Add(roleId);
                    //continue;
                }
                else
                    p.GrantedRoles.Remove(roleId);

                foreach (var vmArea in vmAssembly.AreaPermissons)
                {
                    vmArea.Name = vmArea.Name == null ? "" : vmArea.Name;
                    var pArea = p.AreaPermissions.SingleOrDefault(a => a.AreaName.ToUpper() == vmArea.Name.ToUpper());
                    if (pArea == null)
                    {
                        //if (!vmArea.AccessGranted && !vmArea.AnyChildrenEnabled)
                        //{
                        //    continue;
                        //}

                        pArea = new PermissionArea();
                        pArea.AreaName = vmArea.Name;
                        p.AreaPermissions.Add(pArea);
                    }

                    if (vmArea.AccessGranted && vmArea.AllChildrenEnabled)
                    {
                        pArea.GrantedRoles.Add(roleId);
                        //continue;
                    }
                    else
                        pArea.GrantedRoles.Remove(roleId);

                    foreach (var vmCont in vmArea.ControllerPermissons)
                    {
                        var pCont = pArea.ControllerPermissons.SingleOrDefault(a => a.ControllerName.ToUpper() == vmCont.Name.ToUpper());
                        if (pCont == null)
                        {
                            //if (!vmCont.AccessGranted && !vmCont.AnyChildrenEnabled)
                            //{
                            //    continue;
                            //}

                            pCont = new PermissionController();
                            pCont.ControllerName = vmCont.Name;
                            pArea.ControllerPermissons.Add(pCont);
                        }

                        if (vmCont.AccessGranted && vmCont.AllChildrenEnabled)
                        {
                            pCont.GrantedRoles.Add(roleId);
                            //continue;
                        }
                        else
                            pCont.GrantedRoles.Remove(roleId);

                        foreach (var vmAction in vmCont.ActionPermissons)
                        {
                            var pAction = pCont.ActionPermissons.SingleOrDefault(a => a.ActionName.ToUpper() == vmAction.Name.ToUpper() &&
                                            a.Method == vmAction.Method);
                            if (pAction == null)
                            {
                                //if (!vmAction.AccessGranted && !vmAction.AnyChildrenEnabled)
                                //{
                                //    continue;
                                //}

                                pAction = new PermissionAction();
                                pAction.ActionName = vmAction.Name;
                                pAction.Method = vmAction.Method;
                                pCont.ActionPermissons.Add(pAction);
                            }

                            if (vmAction.AccessGranted && vmAction.AllChildrenEnabled)
                            {
                                pAction.GrantedRoles.Add(roleId);
                               // continue;
                            }
                            else
                                pAction.GrantedRoles.Remove(roleId);
                        }
                    }
                }
            }

            await _repository.UpdateAsync(toUpdate);
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

        private async Task<List<PermissionAssembly>> GetCurrentPermissions()
        {
            var returnVal = await _repository.FindAllAsync(new GetAllPermissions());

            return returnVal.ToList();
        }
    }
}
