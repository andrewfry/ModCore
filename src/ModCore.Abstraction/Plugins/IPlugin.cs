﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using ModCore.Models.Plugins;

namespace ModCore.Abstraction.Plugins
{
    public interface IPlugin
    {
        string Name { get; }

        string Version { get; }

        string AssemblyName { get; }

        string Description { get; }

        //IDictionary<int, Action<IRouteBuilder>> Routes { get; }

        //IDictionary<int, Action<IPluginRoutes>> Routes { get; }

        ICollection<IPluginRoute> Routes { get; }

        ICollection<ServiceDescriptor> Services { get; }


        PluginInstallResult Install();

        PluginInstallResult UnInstall();

    }
}
