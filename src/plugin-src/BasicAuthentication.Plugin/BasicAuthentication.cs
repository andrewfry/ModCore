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
using ModCore.Models.Core;
using ModCore.Abstraction.Plugins.Builtins;

namespace BasicAuthentication.Plugin
{
    public class BasicAuthentication : BasePlugin, IPlugin
    {

        public override string Name
        {
            get
            {
                return "BasicAuthentication";
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
                return "This plugin provides authentication by validating the User object in the DB. The authentication sessions are tied to the type of option session configured (InMemory vs Distributed).";
            }
        }

        public ICollection<IPluginRoute> Routes
        {
            get
            {
                var routes = new List<IPluginRoute>();

                routes.MapPluginRoute(
                 name: "basic_authentication_register",
                 template: "Register",
                 defaults: new { controller = "User", action = "Register" },
                 plugin: new BasicAuthentication());


                return routes;
            }
        }

        public ICollection<IPluginDependency> Dependencies { get { return new List<IPluginDependency>(); } }

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

            settings.EnsureDefaultSettingAsync(BasicAuthentication.BuiltInSettings.Enabled, true);
            settings.EnsureDefaultSettingAsync(BasicAuthentication.BuiltInSettings.RegisterUserEnabled, true);
            settings.EnsureDefaultSettingAsync(BasicAuthentication.BuiltInSettings.UserEmailValidationRequired, true);


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
            public static SettingRegionPair Enabled => new SettingRegionPair("GENERAL", "ENABLED");

            public static SettingRegionPair RegisterUserEnabled => new SettingRegionPair("GENERAL", "RegisterUserEnabled");

            public static SettingRegionPair UserEmailValidationRequired => new SettingRegionPair("GENERAL", "UserEmailValidationRequired");
        }
    }
}
