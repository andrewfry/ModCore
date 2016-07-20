using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModCore.Abstraction.Plugins
{
    public interface IPluginRoute
    {
         string RouteName { get; }
         string RouteTemplate { get; }
         RouteValueDictionary Defaults { get; }
         IDictionary<string, object> Constraints { get; }
         RouteValueDictionary DataTokens { get; }
    }
}
