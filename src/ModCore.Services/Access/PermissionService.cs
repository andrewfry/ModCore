using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using ModCore.Abstraction.DataAccess;
using ModCore.Abstraction.Plugins;
using ModCore.Abstraction.Services.Access;
using ModCore.Abstraction.Site;
using ModCore.Core.HelperExtensions;
using ModCore.Models.Access;
using ModCore.Models.Enum;
using ModCore.Services.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ModCore.Services.Access
{
    public class PermissionService : BaseServiceAsync<Permission>, IPermissionService
    {

        private readonly IActionDescriptorCollectionProvider _actionDescriptorProvider;

        public PermissionService(IDataRepositoryAsync<Permission> repos, IMapper mapper, ILog logger, IActionDescriptorCollectionProvider actionDescriptorProvider) :
            base(repos, mapper, logger)
        {
            _actionDescriptorProvider = actionDescriptorProvider;
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



    }
}
