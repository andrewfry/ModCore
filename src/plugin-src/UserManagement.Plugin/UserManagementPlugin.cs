using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using ModCore.Abstraction.DataAccess;
using ModCore.Abstraction.Plugins;
using ModCore.Abstraction.Plugins.Builtins;
using ModCore.Core.Plugins;
using ModCore.Core.Plugins.Descriptions;
using ModCore.DataAccess.MongoDb;
using ModCore.Models.Core;
using ModCore.Models.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserManagement.Plugin
{
    public class UserManagementPlugin : BasePlugin, IPlugin
    {

        public string Name
        {
            get
            {
                return "UserManagement";
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
                return "Create, edit, deleted and management user accounts.";
            }
        }

        public ICollection<IPluginRoute> Routes
        {
            get
            {
                var routes = new List<IPluginRoute>();
                routes.MapPluginRoute(
                name: "userManagementDefaultAdmin",
                template: "{area=Admin}/{controller=UserManagement}/{action=Index}/{id?}",
                plugin: new UserManagementPlugin());


                return routes;
            }
        }

        public ICollection<ServiceDescriptor> Services
        {
            get
            {
                var list = new List<ServiceDescriptor>();
                //list.Add(ServiceDescriptor.Transient<IPermissionService, PermissionService>());
                return list;
            }
        }

        public FilterCollection Filters
        {
            get
            {
                var list = new FilterCollection();
                //list.Add(typeof(PermissionFilter));
                return list;
            }
        }

        public ICollection<IPluginDependency> Dependencies
        {
            get
            {
                return new List<IPluginDependency>()
                    {
                       // new RequiredPlugin(new AuthenticationService())
                    };
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

           // settings.EnsureDefaultSettingAsync(RoleBasedPermission.BuiltInSettings.AllowAnonymous, true);
           
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
       //     public static SettingRegionPair AllowAnonymous => new SettingRegionPair("GENERAL", "ALLOW_ANONYMOUS");
        }
    }
}
