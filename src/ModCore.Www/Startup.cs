﻿using Microsoft.AspNetCore.Builder;
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
using ModCore.Models.Themes;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ModCore.Models.Page;
using System.Collections.Generic;
using Microsoft.Extensions.FileProviders;
using System.IO;
using ModCore.Services.Access;
using ModCore.Abstraction.Services.Access;
using ModCore.Models.Access;
using AutoMapper;
using ModCore.Services.Mappings;
using ModCore.Specifications.Themes;
using ModCore.Specifications.Plugins;
using System.Threading.Tasks;
using ModCore.Abstraction.Services.Site;
using ModCore.Services.Site;
using ModCore.Core.Filters;
using Microsoft.AspNetCore.Http.Features;
using ModCore.Specifications.Access;
using ModCore.Models.Site;
using ModCore.ViewModels.Access;

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


            services.AddTransient<ILogger, SiteLogger>();
            services.AddTransient<ILog, SiteLogger>();
            services.AddTransient<ISessionManager, SessionManager>();
            services.AddTransient<ILayoutManager, LayoutManager>();
            services.AddSingleton<ISiteSettingsManagerAsync, SiteSettingsManager>();
            services.AddTransient<IBaseViewModelProvider, DefaultBaseViewModelProvider>();
            services.AddTransient<IMenuManager, MenuManager>();

            //Persistent Data Repositories
            services.AddTransient<IDataRepository<InstalledPlugin>, MongoDbRepository<InstalledPlugin>>();
            services.AddTransient<IDataRepository<SiteTheme>, MongoDbRepository<SiteTheme>>();
            services.AddTransient<IDataRepositoryAsync<User>, MongoDbRepository<User>>();
            services.AddTransient<IDataRepositoryAsync<Log>, MongoDbRepository<Log>>();
            services.AddTransient<IDataRepositoryAsync<SiteSetting>, MongoDbRepository<SiteSetting>>();
            services.AddTransient<IDataRepositoryAsync<UserActivity>, MongoDbRepository<UserActivity>>();
            services.AddTransient<IDataRepositoryAsync<Role>, MongoDbRepository<Role>>();
            services.AddTransient<IDataRepositoryAsync<Menu>, MongoDbRepository<Menu>>();

            //Adding the business logic Services
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<ILogService, LogService>();
            services.AddTransient<IUserActivityService, UserActivityService>();
            services.AddTransient<ISessionService, SessionService>();
            services.AddTransient<IRoleService, RoleService>();

            //TODO - Double check if this is pulling profiles from the plugins
            services.AddAutoMapper();

            //TEST
            RunTestData(services);

            var mvcBuilder = services.AddMvc(options =>
            {
                var srvProvider = services.BuildServiceProvider();
                options.Filters.Add(new UserActivityFilter(srvProvider.GetRequiredService<ISiteSettingsManagerAsync>(), srvProvider.GetRequiredService<IUserActivityService>()));
            });

            //Adding the pluginservices 
            services.AddPlugins(mvcBuilder);
            services.AddPluginManager(Configuration, _hostingEnvironment);
            services.AddActivePluginServices(mvcBuilder);
            services.AddThemeManager(Configuration, _hostingEnvironment);
            services.AddAutoMapperClassesFromPlugin();
            //setting up the sesssion
            services.AddMemoryCache();
            services.AddSession(options =>
            {
                var srvProvider = services.BuildServiceProvider();
                var siteSettings = srvProvider.GetRequiredService<ISiteSettingsManagerAsync>();
                var sessionGuid = siteSettings.GetSettingOrDefault<string>(BuiltInSettings.SessionId, "defaultId").Result;
                var sessionTimeOut = siteSettings.GetSettingOrDefault<int>(BuiltInSettings.SessionTimeOut, 20).Result;

                options.IdleTimeout = TimeSpan.FromMinutes(sessionTimeOut);
                options.CookieName = ".Modcore-" + sessionGuid;
            });
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient(typeof(ISession), serviceProvider =>
            {
                var httpContextAccessor = serviceProvider.GetService<IHttpContextAccessor>();
                return httpContextAccessor.HttpContext.Session;
            });

        }

        public void RunTestData(IServiceCollection services)
        {
            var srcProvider = services.BuildServiceProvider();
            /* var repos = srcProvider.GetService<IDataRepository<Page>>()*/
            ;

            //repos.Insert(new Page
            //{
            //    Active = true,
            //    HTMLContent = "<h1>Sample page content 1<h2>",
            //    FriendlyURL = "sample-page",
            //    PageName = "Sample"
            //});
            //repos.Insert(new Page
            //{
            //    Active = true,
            //    HTMLContent = "<h2>Another page</h2>",
            //    FriendlyURL = "sample-page/sub-page",
            //    PageName = "SubSample"
            //});
            //});
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

            var repos1 = srcProvider.GetService<IDataRepository<InstalledPlugin>>();
            if (repos1.Find(new ByAssemblyName("BasicAuthentication.Plugin")) == null)
            {
                repos1.Insert(new InstalledPlugin
                {
                    Active = true,
                    DateInstalled = DateTime.UtcNow,
                    Installed = true,
                    PluginAssemblyName = "BasicAuthentication.Plugin",
                    PluginName = "BasicAuthentication",
                    PluginVersion = "1.0"
                });
            }
            if (repos1.Find(new ByAssemblyName("Pages.Plugin")) == null)
            {
                repos1.Insert(new InstalledPlugin
                {
                    Active = true,
                    DateInstalled = DateTime.UtcNow,
                    Installed = true,
                    PluginAssemblyName = "Pages.Plugin",
                    PluginName = "Pages",
                    PluginVersion = "1.0"
                });
            }


            var usrService = srcProvider.GetService<IUserService>();
            var user = usrService.GetByEmailAsync("test@test.com");
            if (user.Result == null)
            {
                var testUser = usrService.CreateNewUser(new vRegister
                {
                    EmailAddress = "test@test.com",
                    Password = "test",
                    ConfirmPassword = "test",
                    FirstName = "Test",
                    LastName = "Test",
                });
            }


            var themes = srcProvider.GetService<IDataRepository<SiteTheme>>();
            var theme = themes.FindAll(new ActiveSiteTheme());
            if (theme.Count == 0)
            {
                themes.Insert(new SiteTheme
                {
                    Description = "Sample Theme for test purposes.",
                    DisplayName = "Sample Theme",
                    ThemeName = "Sample",
                    ThemeVersion = "1.0",
                    Active = true,

                });
            }

            var roleservice = srcProvider.GetService<IDataRepositoryAsync<Role>>();
            var roles = roleservice.FindAllAsync(new GetAllRoles()).Result;
            if (roles.Count == 0)
            {
                roleservice.InsertAsync(new Role
                {
                    Name = "Admin"
                });
                roleservice.InsertAsync(new Role
                {
                    Name = "Bloggers"
                });
            }


            var siteMenu = new Menu()
            {
                Name = BuiltInMenus.AdminMenu,
                MenuItems = new List<MenuItem>()
                  {
                      new MenuItem()
                      {
                           Name = "Dashboard",
                            IconClass = "menu-item-icon mdi mdi-gauge",
                             Id = Guid.NewGuid().ToString(),
                              Order = 10,
                              Url = "/"
                      },
                      new MenuItem()
                      {
                           Name = "Settings",
                            IconClass = "menu-item-icon mdi mdi-settings",
                             Id = Guid.NewGuid().ToString(),
                              Order = 20,
                               Children = new List<MenuItem>()
                               {
                                    new MenuItem()
                                    {
                                        Name = "Plugins",
                            IconClass = "menu-item-icon mdi mdi-power-socket",
                             Id = Guid.NewGuid().ToString(),
                              Order = 10,
                              Url = "/Admin/Plugin",

                                    },
                                    new MenuItem()
                                    {
                                        Name = "Site Settings",
                            IconClass = "menu-item-icon mdi mdi-settings",
                             Id = Guid.NewGuid().ToString(),
                              Order = 20,
                              Url = "/Admin/SiteSettings",

                                    },
                                    new MenuItem()
                                    {
                                        Name = "Logs",
                            IconClass = "menu-item-icon mdi mdi-file-multiple",
                             Id = Guid.NewGuid().ToString(),
                              Order = 30,
                              Url = "/Admin/Logs",

                                    }
                               }

                      },
                  }
            };

            var menuManager = srcProvider.GetService<IMenuManager>();
            var menuRepos = srcProvider.GetService<IDataRepositoryAsync<Menu>>();

            var menuFromDb = menuManager.GetMenuByName(BuiltInMenus.AdminMenu);
            if (menuFromDb == null)
                menuRepos.InsertAsync(siteMenu);

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
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(
                Path.Combine(Directory.GetCurrentDirectory(), @"Themes")),
                RequestPath = new PathString("/Themes")
            });
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(
                Path.Combine(Directory.GetCurrentDirectory(), @"Plugins")),
                RequestPath = new PathString("/Plugins")
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
            app.RegisterActivePlugins();
            app.UseDefaultSettings();


        }



    }
}
