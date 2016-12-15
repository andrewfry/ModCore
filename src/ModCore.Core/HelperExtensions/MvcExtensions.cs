using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Razor.Compilation;
using Microsoft.AspNetCore.Routing;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.DependencyModel;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModCore.Abstraction.DataAccess;
using ModCore.Abstraction.Plugins;
using ModCore.Abstraction.Site;
using ModCore.Abstraction.Themes;
using ModCore.Core.Controllers;
using ModCore.Core.HelperExtensions;
using ModCore.Core.Plugins;
using ModCore.Core.Routers;
using ModCore.Core.Site;
using ModCore.Core.Themes;
using ModCore.Models.Page;
using ModCore.Models.Plugins;
using ModCore.Models.Themes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
                throw new InvalidOperationException("Plugin manager was not set up correctly, missing IPluginManager service.");
            }

            var pluginManager = app.ApplicationServices.GetService<IPluginManager>();

            var pluginRouters = app.ApplicationServices.GetServices<IPluginRouter>();
            foreach (var tempRouter in pluginRouters.OrderBy(a => a.Position))
            {
                routes.Routes.Add(tempRouter);
            }

            configureRoutes(routes);
               
            //var cmsPage = app.ApplicationServices.GetService<IPageService>();
            // routes.Routes.Add(new CmsPageRoute(cmsPage,routes.DefaultHandler));
            //Disable for now.
            //routes.Routes.Insert(0, AttributeRouting.CreateAttributeMegaRoute(app.ApplicationServices));
            return app.UseRouter(routes.Build());
        }

        public static IApplicationBuilder UseDefaultSettings(this IApplicationBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            var pluginManager = app.ApplicationServices.GetService<IPluginManager>();
            var siteSettings = app.ApplicationServices.GetService<ISiteSettingsManagerAsync>();

            siteSettings.EnsureDefaultSettingAsync(BuiltInSettings.LogLevel, LogLevel.Warning);
            siteSettings.EnsureDefaultSettingAsync(BuiltInSettings.AuthenticationLockOut, 3);
            siteSettings.EnsureDefaultSettingAsync(BuiltInSettings.SessionId, Guid.NewGuid().ToString());
            siteSettings.EnsureDefaultSettingAsync(BuiltInSettings.SessionTimeOut, 20);
            siteSettings.EnsureDefaultSettingAsync(BuiltInSettings.UsrActTrking, true);
            siteSettings.EnsureDefaultSettingAsync(BuiltInSettings.UsrActTrkingDetailed, true);
            siteSettings.EnsureDefaultSettingAsync(BuiltInSettings.UsrActTrkingBaseModelRecord, false);

            return app;
        }

        public static IApplicationBuilder RegisterActivePlugins(this IApplicationBuilder app)
        {
            var pluginManager = app.ApplicationServices.GetService<IPluginManager>();
            pluginManager.RegisterPluginList(pluginManager.ActivePlugins);
            return app;

        }

        public static IServiceCollection AddPlugins(this IServiceCollection services, IMvcBuilder mvcBuilder)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddTransient<IControllerActivator, ValidateControllerActivator>();
            services.AddTransient<IPluginLog, PluginLogger>();
            services.AddTransient<IPluginSettingsManager, PluginSettingsManager>();
            services.AddTransient<IAssemblyManager, PluginAssemblyManager>();
            services.AddTransient<IRouteBuilder, PluginRouteBuilder>();
            services.AddSingleton<IActionDescriptorCollectionProvider, PluginActionDescriptorCollectionProvider>();


            mvcBuilder.AddRazorOptions(a => a.ViewLocationExpanders.Add(new PluginViewLocationExpander()));

            return services;
        }

        public static IServiceCollection AddPluginManager(this IServiceCollection services, IConfigurationRoot configRoot, IHostingEnvironment env)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddSingleton<IPluginManager, PluginManager>(srcProvider =>
            {
                var assbly = srcProvider.GetRequiredService<IAssemblyManager>();
                var repos = srcProvider.GetRequiredService<IDataRepository<InstalledPlugin>>();
                var appMgr = srcProvider.GetRequiredService<ApplicationPartManager>();

                return new PluginManager(assbly, configRoot, env, repos, appMgr);
            });


            services.TryAddEnumerable(
            ServiceDescriptor.Transient<IApplicationModelProvider, PluginApplicationModelProvider>());

            return services;
        }

        public static IServiceCollection AddActivePluginServices(this IServiceCollection services, IMvcBuilder mvcBuilder)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            var srvProvider = services.BuildServiceProvider();
            var pluginManager = srvProvider.GetRequiredService<IPluginManager>();
            //var accessor = srvProvider.GetRequiredService<IOptions<RazorViewEngineOptions>>();

            //foreach (var assemblyTuple in pluginManager.ActivePluginAssemblies)
            //{
            //    foreach (var assembly in assemblyTuple.Item2)
            //    {
            //        // var metaReference = MetadataReference.CreateFromAssembly(assembly);
            //        var metaReference = MetadataReference.CreateFromFile(assembly.Location);
            //        accessor.Value.AdditionalCompilationReferences.Add(metaReference);
            //    }
            //}

            mvcBuilder.ConfigureApplicationPartManager(manager =>
            {
                manager.FeatureProviders.Add(new PluginAssemblyMetadataReferenceFeatureProvider(pluginManager));
            });

            foreach (var plugin in pluginManager.ActivePlugins)
            {
                pluginManager.ActivatePlugin(plugin);               
            }

            //var accessor = srvProvider.GetRequiredService<IApplicationFeatureProvider<MetadataReferenceFeature>>();

            //foreach (var assemblyTuple in pluginManager.ActivePluginAssemblies)
            //{
            //    var pluginAssembly = assemblyTuple.Item2.SingleOrDefault(a => a.GetType().Namespace == assemblyTuple.Item1.AssemblyName);
            //    var dependencies = assemblyTuple.Item2.Where(a => a.GetType().Namespace != assemblyTuple.Item1.AssemblyName).ToList();
            //    var assemblyPart = new AssemblyPart(pluginAssembly);
            //    var metaDataRefFeature = new MetadataReferenceFeature();

            //    foreach (var assembly in dependencies)
            //    {
            //        var metaReference = MetadataReference.CreateFromFile(assembly.Location);
            //        metaDataRefFeature.MetadataReferences.Add(metaReference);
            //    }
            //}


            foreach (var srv in pluginManager.ActivePluginServices)
            {
                if (srv.ServiceType == typeof(IPluginRouter))
                {
                    services.TryAddEnumerable(srv);
                    continue;
                }
                services.Add(srv);
 
            }

            return services;
        }

        public static IServiceCollection AddThemeManager(this IServiceCollection services, IConfigurationRoot configRoot, IHostingEnvironment env)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddSingleton<IThemeManager, ThemeManager>(srcProvider =>
            {
                var repos = srcProvider.GetRequiredService<IDataRepository<SiteTheme>>();

                return new ThemeManager(configRoot, env, repos);
            });

            return services;
        }

        public static IServiceCollection ConfigureMvcWithSettings(this IServiceCollection services, IMvcBuilder mvcBuilder)
        {
            return services;
        }


        public static void AddAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(DependencyContext.Default);
        }

        public static void AddAutoMapper(this IServiceCollection services, DependencyContext dependencyContext)
        {
            AddAutoMapperClasses(services, dependencyContext.RuntimeLibraries
                .SelectMany(lib => lib.GetDefaultAssemblyNames(dependencyContext).Select(Assembly.Load)));
        }

        private static void AddAutoMapperClasses(IServiceCollection services, IEnumerable<Assembly> assembliesToScan)
        {
            assembliesToScan = assembliesToScan as Assembly[] ?? assembliesToScan.ToArray();

            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfiles(assembliesToScan);

            });

            services.AddSingleton<IMapper>(sp => mapperConfiguration.CreateMapper());

        }

        public static void AddAutoMapperClassesFromPlugin(this IServiceCollection services)
        {
            var srvProvider = services.BuildServiceProvider();
            var pluginManager = srvProvider.GetRequiredService<IPluginManager>();


            var assemblyList = DependencyContext.Default.RuntimeLibraries
                .SelectMany(lib => lib.GetDefaultAssemblyNames(DependencyContext.Default).Select(Assembly.Load)).ToList();

            foreach (var plugin in pluginManager.ActiveAssemblies)
            {
                assemblyList.Add(plugin);
            }
            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfiles(assemblyList);

            });

            services.Replace(ServiceDescriptor.Singleton<IMapper>(sp => mapperConfiguration.CreateMapper()));

        }


    }
}
