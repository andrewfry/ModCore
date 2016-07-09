using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using ModCore.Abstraction.DataAccess;
using ModCore.Abstraction.Plugins;
using ModCore.Abstraction.Site;
using ModCore.DataAccess.InMemory;
using ModCore.Models.Core;
using ModCore.Core.Controllers;
using ModCore.Core.Plugins;
using ModCore.Core.Site;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ModCore.Core;
using System.Reflection;

namespace ModCore.Www
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; }

        private IPluginManager _pluginManager;
        private IHostingEnvironment _hostingEnvironment;


        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();

            _hostingEnvironment = env;
            _pluginManager = new PluginManager(new PluginAssemblyManager());
        }



        public void ConfigureServices(IServiceCollection services)
        {
            IMvcBuilder mvcBuilder = services.AddMvc();

            ConfigurePlugins(services, mvcBuilder);
            
            this._hostingEnvironment.WebRootFileProvider = this.CreateCompositeFileProvider();


            services.AddMvc();
            services.AddTransient<IControllerActivator, ValidateControllerActivator>();

            services.AddTransient<IPluginLog, PluginLogger>();
            services.AddTransient<ILog, SiteLogger>();
            services.AddTransient<ISessionManager, SessionManager>();
            services.AddTransient<IPluginSettingsManager, PluginSettingsManager>();
            services.AddTransient<ISiteSettingsManager, SiteSettingsManager>();

            services.AddTransient<IDataRepository<Log>, InMemoryRepository<Log>>();
        }


        public void ConfigurePlugins(IServiceCollection services, IMvcBuilder mvcBuilder)
        {
            var pluginDir = Configuration.GetValue<string>("PluginDir");
            var fullPluginDir = Path.Combine(_hostingEnvironment.ContentRootPath, pluginDir);

            _pluginManager.SetPluginDirPath(fullPluginDir);
            _pluginManager.GetActivePluginAssemblies();

            mvcBuilder.AddRazorOptions(a => a.ViewLocationExpanders.Add(new PluginViewLocationExpander()));

            foreach (var assembly in _pluginManager.GetActivePluginAssemblies())
            {
                mvcBuilder.AddApplicationPart(assembly);
                mvcBuilder.AddRazorOptions(a => a.FileProviders.Add(new EmbeddedFileProvider(assembly, assembly.GetName().Name)));
            }

            foreach (var plugin in _pluginManager.GetActivePlugins())
            {
                foreach (var plugService in plugin.Services)
                {
                    services.Add(plugService);
                }

                plugin.Install(); //TODO complete
            }
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseDeveloperExceptionPage();
            app.UseStatusCodePages();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                     "default",
                    "{controller=Home}/{action=Index}/{id?}",
                    null,
                    null,
                    new { Namespace = this.GetType().GetTypeInfo().Assembly.GetName().Name });
            });

            app.UseRoutesFromPlugins(_pluginManager);
        }

        //private IDictionary<int, List<Action<IRouteBuilder>>> GetRouteByPriority()
        //{
        //    var routePriorities = new Dictionary<int, List<Action<IRouteBuilder>>>();

        //    foreach (var plugin in _pluginManager.GetActivePlugins())
        //    {
        //        if (plugin.Routes != null)
        //        {
        //            foreach (var routeRegistrarByPriority in plugin.Routes)
        //            {
        //                if (!routePriorities.ContainsKey(routeRegistrarByPriority.Key))
        //                    routePriorities.Add(routeRegistrarByPriority.Key, new List<Action<IRouteBuilder>>());

        //                routePriorities[routeRegistrarByPriority.Key].Add(routeRegistrarByPriority.Value);
        //            }
        //        }
        //    }

        //    return routePriorities.OrderBy(routeRegistrarSetByPriority => routeRegistrarSetByPriority.Key).ToDictionary(a => a.Key, a => a.Value);
        //}

        private IFileProvider CreateCompositeFileProvider()
        {
            IFileProvider[] fileProviders = new IFileProvider[] {
                        this._hostingEnvironment.WebRootFileProvider
                      };

            return new CompositeFileProvider(
              fileProviders.Concat(
                _pluginManager.GetActivePluginAssemblies().Select(a => new EmbeddedFileProvider(a, a.GetName().Name))
              )
            );
        }
    }
}
