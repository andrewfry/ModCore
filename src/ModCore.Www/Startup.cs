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
using Microsoft.AspNetCore.Mvc.Infrastructure;
using ModCore.Models.Plugins;

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
        }



        public void ConfigureServices(IServiceCollection services)
        {
            var mvcBuilder = services.AddMvc();
            services.AddTransient<IControllerActivator, ValidateControllerActivator>();
            services.AddTransient<IPluginLog, PluginLogger>();
            services.AddTransient<ILog, SiteLogger>();
            services.AddTransient<ISessionManager, SessionManager>();
            services.AddTransient<IPluginSettingsManager, PluginSettingsManager>();
            services.AddTransient<ISiteSettingsManager, SiteSettingsManager>();
            services.AddTransient<IDataRepository<Log>, InMemoryRepository<Log>>();
            services.AddTransient<IAssemblyManager, PluginAssemblyManager>();
            services.AddTransient<IActionDescriptorCollectionProvider, PluginActionDescriptorCollectionProvider>();

            services.AddTransient<IDataRepository<InstalledPlugin>, InMemoryRepository<InstalledPlugin>>();

            services.AddSingleton<IPluginManager, PluginManager>(srcProvider =>
            {
                var repos = srcProvider.GetService<IDataRepository<InstalledPlugin>>();
                repos.Insert(new InstalledPlugin
                {
                    Active = true,
                    DateInstalled = DateTime.UtcNow,
                    Id = "1",
                    Installed = true,
                    PluginAssemblyName = "Blog.Plugin",
                    PluginName = "Blog",
                    PluginVersion = "1.0"
                });


                return new PluginManager(srcProvider.GetService<IAssemblyManager>(), Configuration, _hostingEnvironment, repos);
            });

            ConfigurePlugins(services, mvcBuilder);
        }


        public void ConfigurePlugins(IServiceCollection services, IMvcBuilder mvcBuilder)
        {
            var sp = services.BuildServiceProvider();
            _pluginManager = sp.GetService<IPluginManager>();

            mvcBuilder.AddRazorOptions(a => a.ViewLocationExpanders.Add(new PluginViewLocationExpander()));

            foreach (var assembly in _pluginManager.ActiveAssemblies)
            {
                mvcBuilder.AddApplicationPart(assembly);
                mvcBuilder.AddRazorOptions(a => a.FileProviders.Add(new EmbeddedFileProvider(assembly, assembly.GetName().Name)));
            }

            foreach (var plugin in _pluginManager.ActivePlugins)
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

            this._hostingEnvironment.WebRootFileProvider = this.CreateCompositeFileProvider();

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

        private IFileProvider CreateCompositeFileProvider()
        {
            IFileProvider[] fileProviders = new IFileProvider[]
                        {
                        this._hostingEnvironment.WebRootFileProvider
                      };

            return new CompositeFileProvider(
              fileProviders.Concat(
                _pluginManager.ActiveAssemblies.Select(a => new EmbeddedFileProvider(a, a.GetName().Name))
              )
            );
        }
    }
}
