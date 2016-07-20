using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using ModCore.Abstraction.Plugins;
using ModCore.Core.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModCore.Core.HelperExtensions
{
    public static class MvcExtensions
    {

        public static IApplicationBuilder UseMvcWithPlugin(
         this IApplicationBuilder app,
         Action<IRouteBuilder> configureRoutes)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            if (configureRoutes == null)
            {
                throw new ArgumentNullException(nameof(configureRoutes));
            }

            // Verify if AddMvc was done before calling UseMvc
            // We use the MvcMarkerService to make sure if all the services were added.
            if (app.ApplicationServices.GetService(typeof(MvcMarkerService)) == null)
            {
                throw new InvalidOperationException("UseMvc called before AddMvc was done.");
            }

            var routes = new PluginRouteBuilder(app)
            {
                DefaultHandler = app.ApplicationServices.GetRequiredService<MvcRouteHandler>(),
                PluginManager = app.ApplicationServices.GetRequiredService<IPluginManager>(),
            };

            if (app.ApplicationServices.GetService(typeof(IPluginManager)) == null)
            {
                throw new InvalidOperationException("Plugin manager was not set up correctly");
            }

            var pluginManager = app.ApplicationServices.GetService<IPluginManager>();

            configureRoutes(routes);

            //Disable for now.
            //routes.Routes.Insert(0, AttributeRouting.CreateAttributeMegaRoute(app.ApplicationServices));

            return app.UseRouter(routes.Build());
        }


        public static IServiceCollection AddPlugins(this IServiceCollection services, IMvcBuilder mvcBuilder)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }



            mvcBuilder.AddRazorOptions(a => a.ViewLocationExpanders.Add(new PluginViewLocationExpander()));


            return services;
        }
    }
}
