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
using ModCore.Abstraction.Plugins.Builtins;

namespace ContentEditor.Plugin
{
    public class ContentEditor : BasePlugin, IPlugin
    {

        public override string Name
        {
            get
            {
                return "ContentEditor";
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
                return "Used to create and manage CSS, CSHTML and CSHTML Layout";
            }
        }

        public ICollection<IPluginRoute> Routes
        {
            get
            {
                var routes = new List<IPluginRoute>();
                routes.MapPluginRoute(
                   name: "contentEditorDefault",
                    template: "ContentEditor/{controller=ContentEditor}/{action=Index}/{id?}",
                    plugin: new ContentEditor());


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


       
    }
}
