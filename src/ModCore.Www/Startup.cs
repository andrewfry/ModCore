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
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using ModCore.Core.HelperExtensions;
using Microsoft.AspNetCore.Mvc.Internal;

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
            services.AddTransient<ILog, SiteLogger>();
            services.AddTransient<ISessionManager, SessionManager>();
            services.AddTransient<ISiteSettingsManager, SiteSettingsManager>();

            //Persistent Data Repositories
            services.AddTransient<IDataRepository<Log>, InMemoryRepository<Log>>();
            services.AddTransient<IDataRepository<InstalledPlugin>, InMemoryRepository<InstalledPlugin>>();

            //Adding the pluginservices 
            services.AddPlugins(mvcBuilder);
            services.AddPluginManager(Configuration, _hostingEnvironment);
            
            RunTestData(services);
        }


        public void RunTestData(IServiceCollection services)
        {
            var srcProvider = services.BuildServiceProvider();

            var repos = srcProvider.GetService<IDataRepository<InstalledPlugin>>();

            repos.Insert(new InstalledPlugin
            {
                Active = false,
                DateInstalled = DateTime.UtcNow,
                Id = "1",
                Installed = true,
                PluginAssemblyName = "Blog.Plugin",
                PluginName = "Blog",
                PluginVersion = "1.0"
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {

          //  this._hostingEnvironment.WebRootFileProvider = this.CreateCompositeFileProvider();

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

            app.UseMvcWithPlugin(routes =>
            {
                routes.MapRoute(
                     "default",
                    "{controller=Home}/{action=Index}/{id?}",
                    null,
                    null,
                    new { Namespace = this.GetType().GetTypeInfo().Assembly.GetName().Name });
            });


        }

        //private IFileProvider CreateCompositeFileProvider()
        //{
        //    IFileProvider[] fileProviders = new IFileProvider[]
        //                {
        //                this._hostingEnvironment.WebRootFileProvider
        //              };

        //    return new CompositeFileProvider(
        //      fileProviders.Concat(
        //        _pluginManager.ActiveAssemblies.Select(a => new EmbeddedFileProvider(a, a.GetName().Name))
        //      )
        //    );
        //}
    }
}
