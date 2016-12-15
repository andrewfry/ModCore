using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using ModCore.Abstraction.Plugins;
using ModCore.Core.Plugins;
using ModCore.Models.Plugins;
using RoleBasedPermisison.Abstraction;
using RoleBasedPermisison.Plugin.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoleBasedPermisison.Plugin
{
    public class RoleBasedPermisison : BasePlugin, IPlugin
    {

        public string Name
        {
            get
            {
                return "RoleBasedPermisison";
            }
        }

        public string Version
        {
            get
            {
                return "1.0";
            }
        }

        public string Description
        {
            get
            {
                return "Provide permissions to specific plugins, areas, controllers or actions. Permissions can be granted on a per user basis or based on roles.";
            }
        }

        public ICollection<IPluginRoute> Routes
        {
            get
            {
                var routes = new List<IPluginRoute>();

                return routes;
            }
        }



        public ICollection<ServiceDescriptor> Services
        {
            get
            {
                var list = new List<ServiceDescriptor>();
                list.Add(ServiceDescriptor.Transient<IPermissionManagerService, PermissionService>());
                list.Add(ServiceDescriptor.Transient<IPermissionService, PermissionService>());
                return list;
            }
        }

        public FilterCollection Filters
        {
            get
            {
                var list = new FilterCollection();
                //list.Add(typeof(AdminAuthFilter));
                return list;
            }
        }

        public PluginInstallResult Install()
        {
            return new PluginInstallResult();
        }

        public PluginInstallResult UnInstall()
        {
            return new PluginInstallResult();
        }
    }
}
