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

namespace Emailer.Plugin
{
    public class Emailer : BasePlugin, IPlugin
    {

        public string Name
        {
            get
            {
                return "BasicEmailer";
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
                return "Create, send, and manage emails and email templates";
            }
        }

        public ICollection<IPluginRoute> Routes
        {
            get
            {
                var routes = new List<IPluginRoute>();
                routes.MapPluginRoute(
                name: "emailerDefaultAdmin",
                template: "{area=Admin}/{controller=Emailer}/{action=Index}/{id?}",
                plugin: new Emailer());


                return routes;
            }
        }

        public ICollection<ServiceDescriptor> Services
        {
            get
            {
                var list = new List<ServiceDescriptor>();
                //list.Add(ServiceDescriptor.Transient<IDataRepositoryAsync<PermissionAssembly>, MongoDbRepository<PermissionAssembly>>());

                //list.Add(ServiceDescriptor.Transient<IPermissionManagerService, PermissionService>());
                //list.Add(ServiceDescriptor.Transient<IPermissionService, PermissionService>());
                return list;
            }
        }

        public FilterCollection Filters
        {
            get
            {
                var list = new FilterCollection();
                return list;
            }
        }

        public ICollection<IPluginDependency> Dependencies
        {
            get
            {
                return new List<IPluginDependency>()
                    {
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

            settings.EnsureDefaultSettingAsync(Emailer.BuiltInSettings.Suspend, false);

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
            public static SettingRegionPair Suspend => new SettingRegionPair("GENERAL", "SUSPEND_EMAILER");
        }
    }
}
