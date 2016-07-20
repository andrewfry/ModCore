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

namespace Blog.Plugin
{
    public class Blog : BasePlugin, IPlugin
    {

        public string Name
        {
            get
            {
                return "ModBlog";
            }
        }

        public string Version
        {
            get
            {
                return "1.0";
            }
        }

        //public IDictionary<int, Action<IPluginRoutes>>  Routes
        //{
        //    get
        //    {
        //        var dict = new Dictionary<int, Action<IPluginRoutes>>();
        //        dict.Add(10, a => a.MapPluginRoute(
        //            name: "blogDefault",
        //            template: "Blog/{controller=Home}/{action=Index}/{id?}",
        //            plugin : new Blog()));

        //        return dict;

        //    }
        //}

        public ICollection<IPluginRoute> Routes
        {
            get
            {
                var routes = new List<IPluginRoute>();
                routes.MapPluginRoute(
                   name: "blogDefault",
                    template: "Blog/{controller=Home}/{action=Index}/{id?}",
                    plugin: new Blog());


                return routes;
            }
        }



        public ICollection<ServiceDescriptor> Services
        {
            get
            {
                var list = new List<ServiceDescriptor>();

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
