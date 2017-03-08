using Microsoft.AspNetCore.Routing;
using ModCore.Abstraction.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModCore.Core.Plugins
{


    public class PluginRouteDescriptor : IPluginRouteDescriptor
    {

        public PluginRouteDescriptor(IPlugin plugin, IPluginRoute route)
        {
            this.PluginName = plugin.Name;
            this.PluginVersion = plugin.Version;

            this.RouteName = route.RouteName;
            this.RouteTemplate = route.RouteTemplate;
            this.Defaults = route.Defaults;
            this.Constraints = route.Constraints;
            this.DataTokens = route.DataTokens;
        }

        public string PluginName { get; private set; }
        public string PluginVersion { get; private set; }
        public string RouteName { get; private set; }
        public string RouteTemplate { get; private set; }
        public RouteValueDictionary Defaults { get; private set; }
        public IDictionary<string, object> Constraints { get; private set; }
        public RouteValueDictionary DataTokens { get; private set; }

    }
}
