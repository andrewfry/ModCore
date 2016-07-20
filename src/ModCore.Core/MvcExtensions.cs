using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Builder.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using ModCore.Abstraction.Plugins;
using ModCore.Core.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModCore.Core
{
    public static class MvcOptionsExtensions
    {
        //public static void UseRoutesFromPlugins(this IApplicationBuilder app, IPluginManager pluginManager)
        //{


        //    app.UseMvc(routeBuilder =>
        //    {
        //        var pluginRouteCollection = new PluginRoutes(routeBuilder);
        //        foreach (var plugin in pluginManager.ActivePlugins)
        //        {
        //            foreach (var routeReg in plugin.Routes.Values)
        //            {
        //                routeReg(pluginRouteCollection);
        //            }

        //        }
        //      });

        //}
    }
}
