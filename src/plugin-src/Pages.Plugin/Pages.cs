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

namespace Pages.Plugin
{
    public class Pages : BasePlugin, IPlugin
    {
        public string Name
        {
            get
            {
                return "Pages";
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



        public ICollection<ServiceDescriptor> Services
        {
            get
            {
                var list = new List<ServiceDescriptor>();
                list.Add(ServiceDescriptor.Transient<IDataRepositoryAsync<Page>, MongoDbRepository<Page>>());
                list.Add(ServiceDescriptor.Transient<IPageService, PageService>());
                
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
