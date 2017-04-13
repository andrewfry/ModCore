using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ModCore.Abstraction.Plugins;
using ModCore.Core.Plugins;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.Filters;
using ModCore.Models.Plugins;
using Themes.Plugin.Models;
using ModCore.Abstraction.DataAccess;
using ModCore.DataAccess.MongoDb;
using ModCore.Abstraction.Plugins.Builtins;

namespace Themes.Plugin
{
    public class Themes : BasePlugin, IPlugin
    {
        public override string Name
        {
            get
            {
                return "Themes";
            }
        }

        public override string Version
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
                return "This plugin provides the ability to edit and select the layout and 'look' of each page.";
            }
        }

        public ICollection<IPluginRoute> Routes
        {
            get
            {
                var routes = new List<IPluginRoute>();
                routes.MapPluginRoute(
                name: "pageDefaultAdmin",
                template: "{area=Admin}/{controller=Theme}/{action=Index}/{id?}",
                plugin: new Themes());

                return routes;
            }
        }

        public ICollection<ServiceDescriptor> Services
        {
            get
            {
                var list = new List<ServiceDescriptor>();
                list.Add(ServiceDescriptor.Transient<IDataRepositoryAsync<Theme>, MongoDbRepository<Theme>>());
                //list.Add(ServiceDescriptor.Transient<IThemeService, ThemeService>());
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

        public FilterCollection Filters
        {
            get
            {
                var list = new FilterCollection();

                return list;
            }
        }

        public override PluginResult Install(PluginInstallContext context)
        {
            var result = new PluginResult();
            result.WasSuccessful = true;
            return result;
        }

        public override PluginResult StartUp(PluginStartupContext context)
        {
            var result = new PluginResult();
            result.WasSuccessful = true;
            return result;
        }

        public override PluginResult UnInstall(PluginUninstallContext context)
        {
            var result = new PluginResult();
            result.WasSuccessful = true;
            return result;
        }
    }
}
