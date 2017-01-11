using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using ModCore.Abstraction.DataAccess;
using ModCore.Abstraction.Plugins;
using ModCore.Core.Plugins;
using ModCore.DataAccess.MongoDb;
using ModCore.Models.Core;
using ModCore.Models.Plugins;
using RoleBasedPermission.Plugin.Abstraction;
using RoleBasedPermission.Plugin.Filters;
using RoleBasedPermission.Plugin.Models;
using RoleBasedPermission.Plugin.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoleBasedPermission.Plugin
{
    public class RoleBasedPermission : BasePlugin, IPlugin
    {

        public string Name
        {
            get
            {
                return "RoleBasedPermission";
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
                routes.MapPluginRoute(
                name: "rolePermissionDefaultAdmin",
                template: "{area=Admin}/{controller=RolePermission}/{action=Index}/{id?}",
                plugin: new RoleBasedPermission());


                return routes;
            }
        }

        public ICollection<ServiceDescriptor> Services
        {
            get
            {
                var list = new List<ServiceDescriptor>();
                list.Add(ServiceDescriptor.Transient<IDataRepositoryAsync<PermissionAssembly>, MongoDbRepository<PermissionAssembly>>());

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
                list.Add(typeof(PermissionFilter));
                return list;
            }
        }

        public PluginResult Install(PluginInstallContext context)
        {
            var result = new PluginResult();
            result.WasSuccessful = true;
            return result;
        }

        public PluginResult StartUp(PluginStartupContext context)
        {
            var settings = context.ServiceProvider.GetService<IPluginSettingsManager>();
            settings.SetPlugin(this);

            settings.EnsureDefaultSettingAsync(RoleBasedPermission.BuiltInSettings.AllowAnonymous, true);

            var result = new PluginResult();
            result.WasSuccessful = true;
            return result;
        }

        public PluginResult UnInstall(PluginUninstallContext context)
        {
            var result = new PluginResult();
            result.WasSuccessful = true;
            return result;
        }


        public static class BuiltInSettings
        {
            public static SettingRegionPair AllowAnonymous => new SettingRegionPair("GENERAL", "ALLOW_ANONYMOUS");
        }
    }
}
