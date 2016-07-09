using Microsoft.AspNetCore.Routing;
using ModCore.Abstraction.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModCore.Core.Plugins
{
    public class PluginRouteCollection : IPluginRouteCollection
    {
        private IRouteBuilder _routeBuilder;

        public PluginRouteCollection(IRouteBuilder routeBuilder)
        {
            _routeBuilder = routeBuilder;
        }

        public IRouteBuilder RouteBuider
        {
            get
            {
                return _routeBuilder;
            }
        }
    }
}
