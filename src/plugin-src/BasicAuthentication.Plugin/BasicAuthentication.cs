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
using ModCore.Abstraction.Site;
using BasicAuthentication.Plugin.Services;
using BasicAuthentication.Plugin.Filters;

namespace BasicAuthentication.Plugin
{
    public class BasicAuthentication : BasePlugin, IPlugin
    {

        public string Name
        {
            get
            {
                return "BasicAuthentication";
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
                return "This plugin provides authentication by validating the User object in the DB. The authentication sessions are tied to the type of option session configured (InMemory vs Distributed).";
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
                list.Add(ServiceDescriptor.Transient<IAuthenticationService, AuthenticationService>());
                return list;
            }
        }

        public FilterCollection Filters
        {
            get
            {
                var list = new FilterCollection();
                list.Add(typeof(AdminAuthFilter));
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
