using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using ModCore.Models.Plugins;
using Microsoft.AspNetCore.Mvc.Filters;
using ModCore.Abstraction.Plugins.Builtins;

namespace ModCore.Abstraction.Plugins
{
    public interface IPlugin
    {
        string Name { get; }

        string Version { get; }

        string AssemblyName { get; }

        string Description { get; }

        ICollection<IPluginRoute> Routes { get; }

        ICollection<ServiceDescriptor> Services { get; }

        ICollection<IPluginDependency> Dependencies { get; }

        FilterCollection Filters { get; }

        PluginResult Install(PluginInstallContext context);

        PluginResult StartUp(PluginStartupContext context);

        PluginResult UnInstall(PluginUninstallContext context);

    }
}
