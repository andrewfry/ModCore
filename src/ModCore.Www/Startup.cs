using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using ModCore.Abstraction.DataAccess;
using ModCore.Abstraction.Plugins;
using ModCore.Abstraction.Site;
using ModCore.Models.Core;
using ModCore.Core.Site;
using System;
using System.Reflection;
using ModCore.Models.Plugins;
using ModCore.Core.HelperExtensions;
using Microsoft.AspNetCore.Http;
using ModCore.DataAccess.MongoDb;
using Microsoft.AspNetCore.Authentication.Cookies;
using ModCore.Core.Middleware;
using ModCore.Models.Themes;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ModCore.Services.MongoDb.Access;
using ModCore.Abstraction.Services.Access;
using ModCore.Models.Access;
using AutoMapper;
using ModCore.Services.MongoDb.Mappings;

namespace ModCore.Www
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; }

        private IPluginManager _pluginManager;
        private IHostingEnvironment _hostingEnvironment;
        private string CurrentNameSpace { get { return this.GetType().GetTypeInfo().Assembly.GetName().Name; } }

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
            //Configure Settings
            services.Configure<MongoDbSettings>(options => Configuration.GetSection("MongoDbSettings").Bind(options));

            var mvcBuilder = services.AddMvc(config =>
            {
                config.Filters.Add(new AdminAuthFilter());
            });

            services.AddTransient<ILogger, SiteLogger>();
            services.AddTransient<ILog, SiteLogger>();
            services.AddTransient<ISessionManager, SessionManager>();
            services.AddTransient<ISiteSettingsManager, SiteSettingsManager>();
            services.AddTransient<IBaseViewModelProvider, DefaultBaseViewModelProvider>();

            //Persistent Data Repositories
            services.AddTransient<IDataRepository<InstalledPlugin>, MongoDbRepository<InstalledPlugin>>();
            services.AddTransient<IDataRepository<ActiveTheme>, MongoDbRepository<ActiveTheme>>();

            services.AddTransient<IDataRepositoryAsync<User>, MongoDbRepository<User>>();
            services.AddTransient<IDataRepositoryAsync<Log>, MongoDbRepository<Log>>();

            //Adding the business logic Services
            services.AddTransient<IUserService, UserService>();

            //Adding the pluginservices 
            services.AddPlugins(mvcBuilder);
            services.AddPluginManager(Configuration, _hostingEnvironment);
            services.AddThemeManager(Configuration, _hostingEnvironment);
            var sessionGuid = "TEMP"; //TODO - Get the sessionGuid from the DB

            //setting up the sesssion
            services.AddMemoryCache();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(20);
                options.CookieName = ".Modcore-" + sessionGuid;
            });

            //TODO - Double check if this is pulling profiles from the plugins
            services.AddAutoMapper();

            //TEST
            RunTestData(services);
        }

        public void RunTestData(IServiceCollection services)
        {
            var srcProvider = services.BuildServiceProvider();
            var repos = srcProvider.GetService<IDataRepository<InstalledPlugin>>();

            //repos.Insert(new InstalledPlugin
            //{
            //    Active = false,
            //    DateInstalled = DateTime.UtcNow,
            //    Id = "1",
            //    Installed = true,
            //    PluginAssemblyName = "Blog.Plugin",
            //    PluginName = "Blog",
            //    PluginVersion = "1.0"
            //});

            //var themes = srcProvider.GetService<IDataRepository<ActiveTheme>>();

            //themes.Insert(new ActiveTheme
            //{
            //    Id = "1",
            //    Description = "Sample Theme for test purposes.",
            //    DisplayName = "Sample Theme",
            //    ThemeName = "Sample",
            //    ThemeVersion = "1.0"

            //});
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
            app.UseSession();

            //We are doing our own custom authentication via the session.
            //app.UseCookieAuthentication(new CookieAuthenticationOptions()
            //{
            //    AuthenticationScheme = "ModCoreBasicCookieAuth",
            //    AutomaticAuthenticate = true,
            //    AutomaticChallenge = false,
            //    CookieHttpOnly = true,
            //    ExpireTimeSpan = TimeSpan.FromMinutes(30),
            //    SlidingExpiration = true,
            //    CookieSecure = env.IsDevelopment()
            //                ? CookieSecurePolicy.SameAsRequest
            //                : CookieSecurePolicy.Always,
            //    Events = new CookieAuthenticationEvents
            //    {
            //        // Set other options
            //       // OnValidatePrincipal = LastChangedValidator.ValidateAsync, //not sure if this makes sense since we arent using the claims for data
            //    }
            //});

            app.UseMvcWithPlugin(routes =>
            {
                routes.MapRoute(
                   "areaRoute",
                   "{area:exists}/{controller=Home}/{action=Index}",
                    null,
                    null,
                    new { Namespace = this.CurrentNameSpace });

                routes.MapRoute(
                    "default",
                    "{controller=Home}/{action=Index}/{id?}",
                    null,
                    null,
                    new { Namespace = this.CurrentNameSpace });
            });

            
        }


    }
}
