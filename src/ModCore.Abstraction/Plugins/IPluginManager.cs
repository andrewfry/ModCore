﻿using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using ModCore.Models.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ModCore.Abstraction.Plugins
{
    public interface IPluginManager
    {
        int ActivePluginHash { get; }

        List<PluginError> Errors { get; }

        IList<IPlugin> AvailablePlugins { get; }

        IList<IPlugin> InstalledPlugins { get; }

        IList<IPlugin> ActivePlugins { get; }

        IList<Tuple<IPlugin, IList<Assembly>>> AvailablePluginAssemblies { get; }

        IList<Tuple<IPlugin, IList<Assembly>>> InstalledPluginAssemblies { get; }

        IList<Tuple<IPlugin, IList<Assembly>>> ActivePluginAssemblies { get; }

        IList<FilterDescriptor> GlobalFilterDescriptors { get; }

        IList<Assembly> ActiveAssemblies { get; }

        ICollection<IPluginRouteDescriptor> ActivePluginRoutes { get; }

        IList<ServiceDescriptor> ActivePluginServices { get; }

        PluginResult ActivatePlugin(IPlugin plugin);

        PluginResult DeactivatePlugin(IPlugin plugin);

        void RegisterPluginList(PluginStartupContext context);

        ICollection<IRouter> GetActiveRoutesForPlugins(IRouter defaultHandler, IInlineConstraintResolver inlineConstraintResolver);

    }
}
