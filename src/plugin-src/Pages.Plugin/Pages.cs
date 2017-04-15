using Microsoft.AspNetCore.Routing;
using ModCore.Abstraction.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using ModCore.Models.Plugins;
using ModCore.Core.Plugins;
using Microsoft.AspNetCore.Mvc.Filters;
using Pages.Plugin.Services;
using ModCore.Abstraction.DataAccess;
using ModCore.DataAccess.MongoDb;
using Pages.Plugin.Models;
using Pages.Plugin.Routers;
using Microsoft.AspNetCore.Mvc.Internal;
using ModCore.Abstraction.Plugins.Builtins;
using ModCore.Core.Plugins.Descriptions;

namespace Pages.Plugin
{
    public class Pages : BasePlugin, IPlugin
    {
        public override string Name
        {
            get
            {
                return "Pages";
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
                return "This plugin provides the ability to create and edit HTML pages to display for user interaction.";
            }
        }

        public ICollection<IPluginRoute> Routes
        {
            get
            {
                var routes = new List<IPluginRoute>();
                routes.MapPluginRoute(
                name: "pageDefaultAdmin",
                template: "{area=Admin}/{controller=Page}/{action=Index}/{id?}",
                plugin: new Pages());

                routes.MapPluginRoute(
                name: "pageDefault",
                template: "{controller=Page}/{action=Index}/{id?}",
                plugin: new Pages());

                return routes;
            }
        }


        public ICollection<IPluginDependency> Dependencies
        {
            get
            { return new List<IPluginDependency>() { }; }
        }

        public ICollection<ServiceDescriptor> Services
        {
            get
            {
                var list = new List<ServiceDescriptor>();
                list.Add(ServiceDescriptor.Transient<IDataRepositoryAsync<Page>, MongoDbRepository<Page>>());
                list.Add(ServiceDescriptor.Transient<IPageService, PageService>());
                list.Add(ServiceDescriptor.Transient<IPluginRouter, CmsPageRouter>());
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

        public PluginResult Install(PluginInstallContext context)
        {
            var result = new PluginResult();
            result.WasSuccessful = true;
            return result;
        }

        public PluginResult StartUp(PluginStartupContext context)
        {
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
    }
}
