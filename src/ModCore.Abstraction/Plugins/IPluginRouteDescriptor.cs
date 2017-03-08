using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModCore.Abstraction.Plugins
{
    public interface IPluginRouteDescriptor : IPluginRoute
    {
         string PluginName { get; }

         string PluginVersion { get; }
        
    }
}
