﻿using Microsoft.AspNetCore.Routing;
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
using ModCore.Abstraction.Plugins.Builtins;

namespace Blog.Plugin
{
    public class Blog : BasePlugin, IPlugin
    {

        public override string Name
        {
            get
            {
                return "ModBlog";
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
                return "This plugin is used to post blogs and read them.";
            }
        }

        public ICollection<IPluginRoute> Routes
        {
            get
            {
                var routes = new List<IPluginRoute>();
                routes.MapPluginRoute(
                   name: "blogDefault",
                    template: "Blog/{controller=Plugin}/{action=Index}/{id?}",
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


        public ICollection<IRouter> Routers
        {
            get
            {
                return new List<IRouter>();
            }
        }


        public ICollection<IPluginDependency> Dependencies { get { return new List<IPluginDependency>(); } }

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
