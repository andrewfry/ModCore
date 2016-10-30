using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ModCore.Abstraction.DataAccess;
using ModCore.Abstraction.Plugins;
using ModCore.Abstraction.Site;
using ModCore.DataAccess.InMemory;
using ModCore.Models.Core;
using ModCore.Core.Site;
using System;
using System.Reflection;
using ModCore.Models.Plugins;
using ModCore.Core.HelperExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.IO;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity.MongoDB;
using Microsoft.AspNetCore.Http;
using MongoDB.Driver;
using ModCore.DataAccess.MongoDb;
using Microsoft.AspNetCore.Authentication.Cookies;
using ModCore.Core.Middleware;
using ModCore.Models.Themes;

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

            var mvcBuilder = services.AddMvc();

            services.AddTransient<ILog, SiteLogger>();
            services.AddTransient<ISessionManager, SessionManager>();
            services.AddTransient<ISiteSettingsManager, SiteSettingsManager>();
            services.AddTransient<IBaseViewModelProvider, DefaultBaseViewModelProvider>();

            //Persistent Data Repositories
            services.AddTransient<IDataRepository<Log>, MongoDbRepository<Log>>();
            services.AddTransient<IDataRepository<InstalledPlugin>, MongoDbRepository<InstalledPlugin>>();
            services.AddTransient<IDataRepository<ActiveTheme>, MongoDbRepository<ActiveTheme>>();

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

            var themes = srcProvider.GetService<IDataRepository<ActiveTheme>>();

            themes.Insert(new ActiveTheme
            {
                Id = "1",
                Description = "Sample Theme for test purposes.",
                DisplayName = "Sample Theme",
                ThemeName = "Sample",
                ThemeVersion = "1.0"

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
            app.UseSession();

            app.UseCookieAuthentication(new CookieAuthenticationOptions()
            {
                AuthenticationScheme = "ModCoreBasicCookieAuth",
                LoginPath = new PathString("/Login"),
                AccessDeniedPath = new PathString("/Login"),
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
                CookieHttpOnly = true,
                ExpireTimeSpan = TimeSpan.FromMinutes(30),
                SlidingExpiration = true,
                Events = new CookieAuthenticationEvents
                {
                    // Set other options
                    OnValidatePrincipal = LastChangedValidator.ValidateAsync,
                }

        });

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


        private void AddDefaultTokenProviders(IServiceCollection services)
        {
            var dataProtectionProviderType = typeof(DataProtectorTokenProvider<>).MakeGenericType(typeof(IdentityUser));
            var phoneNumberProviderType = typeof(PhoneNumberTokenProvider<>).MakeGenericType(typeof(IdentityUser));
            var emailTokenProviderType = typeof(EmailTokenProvider<>).MakeGenericType(typeof(IdentityUser));
            AddTokenProvider(services, TokenOptions.DefaultProvider, dataProtectionProviderType);
            AddTokenProvider(services, TokenOptions.DefaultEmailProvider, emailTokenProviderType);
            AddTokenProvider(services, TokenOptions.DefaultPhoneProvider, phoneNumberProviderType);
        }

        private void AddTokenProvider(IServiceCollection services, string providerName, Type provider)
        {
            services.Configure<IdentityOptions>(options =>
            {
                options.Tokens.ProviderMap[providerName] = new TokenProviderDescriptor(provider);
            });

            services.AddSingleton(provider);
        }
    }
}
