using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using ModCore.Abstraction.Plugins;

namespace ModCore.Core.Plugins
{
    public static class MapPluginRoutesExtensions
    {

        public static IPluginRouteCollection MapPluginRoute(this IPluginRouteCollection routeCollectionBuilder,
                                             string name,
                                             string template,
                                             IPlugin plugin)

        {
            MapPluginRoute(routeCollectionBuilder, name, template, null, plugin);
            return routeCollectionBuilder;
        }

        public static IPluginRouteCollection MapPluginRoute(this IPluginRouteCollection routeCollectionBuilder,
                                             string name,
                                             string template,
                                             object defaults,
                                             IPlugin plugin)
        {
            return MapPluginRoute(routeCollectionBuilder, name, template, defaults, null, plugin);
        }

        public static IPluginRouteCollection MapPluginRoute(this IPluginRouteCollection routeCollectionBuilder,
                                             string name,
                                             string template,
                                             object defaults,
                                             object constraints,
                                             IPlugin plugin)
        {
            var inlineConstraintResolver = routeCollectionBuilder.RouteBuider
                 .ServiceProvider
                 .GetRequiredService<IInlineConstraintResolver>();


            routeCollectionBuilder.RouteBuider.Routes.Add(new Route(
                routeCollectionBuilder.RouteBuider.DefaultHandler,
                name,
                template,
                new RouteValueDictionary(defaults),
                new RouteValueDictionary(constraints),
                new RouteValueDictionary(new { Namespace = plugin.AssemblyName }) ,
                inlineConstraintResolver));


            return routeCollectionBuilder;
        }


    }
}
