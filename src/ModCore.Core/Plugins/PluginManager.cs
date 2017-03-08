using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ModCore.Abstraction.DataAccess;
using ModCore.Abstraction.Plugins;
using ModCore.Abstraction.Plugins.Builtins;
using ModCore.Core.Exceptions;
using ModCore.Core.Plugins;
using ModCore.Models.Plugins;
using ModCore.Specifications.Plugins;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace ModCore.Core.Plugins
{
    public class PluginManager : IPluginManager
    {
        private string _pluginDirPath;
        private IDictionary<IPlugin, Assembly> _plugins;
        private IAssemblyManager _assemblyManager;
        private IConfigurationRoot _configurationRoot;
        private IHostingEnvironment _hostingEnvironment;
        private IDataRepository<InstalledPlugin> _repository;
        private ApplicationPartManager _appPartManager;
        private List<PluginError> _errors;
        private ILogger _log;

        private IList<IPlugin> _availablePlugins;
        private IList<IPlugin> _installedPlugins;
        private IList<IPlugin> _activePlugins;


        private IList<Tuple<IPlugin, IList<Assembly>>> _availablePluginAssemnies;
        private IList<Tuple<IPlugin, IList<Assembly>>> _installedPluginAssemblies;
        private IList<Tuple<IPlugin, IList<Assembly>>> _activePluginAssemnies;

        public int ActivePluginHash
        {
            get
            {
                return ActivePlugins.GetHashCode();
            }
        }

        public List<PluginError> Errors
        {
            get
            {
                if (_errors == null)
                    _errors = new List<PluginError>();

                return _errors;
            }
        }

        public IList<IPlugin> AvailablePlugins
        {
            get
            {
                if (_availablePlugins == null)
                {
                    _availablePlugins = AvailablePluginAssemblies.Select(a => a.Item1).ToList();
                }

                return _availablePlugins;
            }
        }

        public IList<IPlugin> InstalledPlugins
        {
            get
            {
                if (_installedPlugins == null)
                {
                    _installedPlugins = InstalledPluginAssemblies.Select(a => a.Item1).ToList();
                }

                return _installedPlugins;
            }
        }

        public IList<IPlugin> ActivePlugins
        {
            get
            {
                if (_activePlugins == null)
                {
                    _activePlugins = ActivePluginAssemblies.Select(a => a.Item1).ToList();
                }

                return _activePlugins;
            }
        }

        public IList<Tuple<IPlugin, IList<Assembly>>> AvailablePluginAssemblies
        {
            get
            {
                if (_availablePluginAssemnies == null)
                {
                    _availablePluginAssemnies = _assemblyManager.GetPluginAndAssemblies(_pluginDirPath);
                    this.Errors.AddRange(_assemblyManager.PluginErrors);
                }

                return _availablePluginAssemnies;
            }
        }

        public IList<Tuple<IPlugin, IList<Assembly>>> InstalledPluginAssemblies
        {
            get
            {
                if (_installedPluginAssemblies == null)
                {
                    var installedFromDb = _repository.FindAll(new AllInstalledPlugins());
                    _installedPluginAssemblies = new List<Tuple<IPlugin, IList<Assembly>>>();

                    foreach (var pluginPair in AvailablePluginAssemblies)
                    {
                        if (installedFromDb.Any(a => a.PluginAssemblyName == pluginPair.Item1.AssemblyName))
                        {
                            _installedPluginAssemblies.Add(pluginPair);
                        }
                    }
                }

                return _installedPluginAssemblies;
            }
        }

        public IList<Tuple<IPlugin, IList<Assembly>>> ActivePluginAssemblies
        {
            get
            {
                if (_activePluginAssemnies == null)
                {
                    var fromDb = _repository.FindAll(new AllActivePlugins());
                    _activePluginAssemnies = new List<Tuple<IPlugin, IList<Assembly>>>();

                    foreach (var pluginPair in InstalledPluginAssemblies)
                    {
                        if (fromDb.Any(a => a.PluginAssemblyName == pluginPair.Item1.AssemblyName))
                        {
                            _activePluginAssemnies.Add(pluginPair);
                        }
                    }

                }

                return _activePluginAssemnies;
            }
        }

        public IList<ServiceDescriptor> ActivePluginServices
        {
            get
            {
                var services = new List<ServiceDescriptor>();

                foreach (var actPlugin in this.ActivePlugins)
                {
                    foreach (var srv in actPlugin.Services)
                    {
                        services.Add(srv);
                    }
                }

                return services;
            }

        }

        public FilterCollection ActivePluginGlobalFilters
        {
            get
            {
                var filters = new FilterCollection();

                foreach (var actPlugin in this.ActivePlugins)
                {
                    foreach (var filt in actPlugin.Filters)
                    {
                        filters.Add(filt);
                    }
                }

                return filters;
            }

        }

        public IList<FilterDescriptor> GlobalFilterDescriptors
        {
            get
            {
                var returnList = new List<FilterDescriptor>();

                foreach (var filter in ActivePluginGlobalFilters)
                {
                    var filtDesc = new FilterDescriptor(filter, 0);
                    returnList.Add(filtDesc);
                }

                return returnList;
            }
        }

        public IList<Assembly> ActiveAssemblies
        {
            get
            {
                return ActivePluginAssemblies.SelectMany(a => a.Item2).ToList();

            }
        }

        public void Refresh()
        {
            _availablePlugins = null;
            _installedPlugins = null;
            _activePlugins = null;

            _availablePluginAssemnies = null;
            _installedPluginAssemblies = null;
            _activePluginAssemnies = null;
        }

        public PluginManager(IAssemblyManager assemblyManager, IConfigurationRoot configurationRoot,
            IHostingEnvironment hostingEnvironment, IDataRepository<InstalledPlugin> repository,
            ApplicationPartManager appPartManager, ILogger log)

        {
            _assemblyManager = assemblyManager;
            _configurationRoot = configurationRoot;
            _hostingEnvironment = hostingEnvironment;
            _appPartManager = appPartManager;
            _log = log;

            _repository = repository;

            var pluginDir = _configurationRoot.GetValue<string>("PluginDir");
            _pluginDirPath = Path.Combine(_hostingEnvironment.ContentRootPath, pluginDir);

            if (string.IsNullOrEmpty(_pluginDirPath))
            {
                throw new ArgumentNullException("pluginDir");
            }

        }


        private PluginResult InstallPlugin(IPlugin plugin) // returns successful if the plugin installed correctly.
        {
            var context = new PluginInstallContext();
            var result = plugin.Install(context);
            if (!result.WasSuccessful)
            {
                return result;
            }

            var installedPlugin = new InstalledPlugin();
            installedPlugin.PluginAssemblyName = plugin.AssemblyName;
            installedPlugin.PluginName = plugin.Name;
            installedPlugin.PluginVersion = plugin.Version;
            installedPlugin.DateInstalled = DateTime.UtcNow;
            installedPlugin.Installed = true;

            _repository.Insert(installedPlugin);


            return new PluginResult
            {
                WasSuccessful = true
            };
        }

        public PluginResult ActivatePlugin(IPlugin plugin)
        {
            var installedPlugin = _repository.Find(new InstalledPluginForPlugin(plugin));
            if (installedPlugin == null)
            {
                var result = InstallPlugin(plugin);
                if (!result.WasSuccessful)
                    return result;
            }

            if (installedPlugin.Active == false)
            {
                installedPlugin.Active = true;
                installedPlugin.DateActivated = DateTime.UtcNow;
                _repository.Update(installedPlugin);
            }

            RegisterAssemblyInPartManager(plugin);

            Refresh();

            return new PluginResult
            {
                WasSuccessful = true
            };
        }

        public PluginResult DeactivatePlugin(IPlugin plugin)
        {
            var installedPlugin = _repository.Find(new InstalledPluginForPlugin(plugin));
            if (installedPlugin == null)
                throw new NullReferenceException($"Can not find installed plugin for {plugin.Name}");

            installedPlugin.Active = false;
            installedPlugin.DateDeactivated = DateTime.UtcNow;
            _repository.Update(installedPlugin);

            RemoveFromAssemblyInPartManager(plugin);

            Refresh();

            return new PluginResult
            {
                WasSuccessful = true
            };
        }

        public void RemoveServices(IPlugin plugin, IMvcCoreBuilder coreBuilder)
        {
            foreach (var srv in plugin.Services)
            {
                coreBuilder.Services.Remove(srv);
            }
        }

        public void AddServices(IPlugin plugin, IMvcCoreBuilder coreBuilder)
        {
            foreach (var srv in plugin.Services)
            {
                coreBuilder.Services.Add(srv);
            }
        }

        public ICollection<IPluginRouteDescriptor> ActivePluginRoutes
        {
            get
            {
                var routeDescs = new List<IPluginRouteDescriptor>();

                foreach (var actPlugin in this.ActivePlugins)
                {
                    foreach(var r in actPlugin.Routes)
                    {
                        routeDescs.Add(new PluginRouteDescriptor(actPlugin, r));
                    }
                }

                return routeDescs;
            }
        }

        public ICollection<IRouter> GetActiveRoutesForPlugins(IRouter defaultHandler, IInlineConstraintResolver inlineConstraintResolver)
        {
            var routes = new List<IRouter>();

            foreach (var actPlugin in this.ActivePlugins)
            {

                foreach (var r in actPlugin.Routes)
                {
                    //var dataTokens = new RouteValueDictionary(new { Namespace = actPlugin.AssemblyName });
                    //foreach (var d in r.DataTokens)
                    //    dataTokens.Add(d.Key, d.Value);


                    routes.Add(new Route(
                   defaultHandler,
                    r.RouteName,
                    r.RouteTemplate,
                    r.Defaults,
                    r.Constraints,
                    r.DataTokens,
                    inlineConstraintResolver
                    ));
                }
            }

            return routes;
        }

        public void RegisterPluginList(PluginStartupContext context)
        {
            foreach (var plugin in this.ActivePlugins)
            {
                RegisterAssemblyInPartManager(plugin);
            }

            if (this.ActivePlugins.Count > 0) Refresh();

            //execute start up tasks needed to run the plugin - this is different from installing the plugin
            foreach (var plugin in this.ActivePlugins)
            {
                RunPluginStartUpTask(context, plugin);
            }
        }

        private bool RunPluginStartUpTask(PluginStartupContext context, IPlugin plugin)
        {
            try
            {
                var result = plugin.StartUp(context);
                if (result.WasSuccessful)
                {
                    _log.LogDebug($"{plugin.Name} was registered successfully.");
                    return true;
                }
                else
                {
                    _log.LogError($"{plugin.Name} failed to register. " + String.Join(" ", result.Errors));
                    return false;
                }
            }
            catch (Exception ex)
            {
                {
                    _log.LogError(new EventId(), $"{plugin.Name} failed to register. ", ex);
                    return false;
                }
            }
        } 


        private bool EnsureAllDependenciesMet(IPlugin plugin)
        {
            var allDependenciesMet = true;
            IPluginDescription notFound = null;

            if (plugin.Dependencies != null && plugin.Dependencies.Any())
            {
                foreach (var dep in plugin.Dependencies)
                {
                    if (dep.Description == null)
                    {
                        _log.LogError($"{plugin.Name} had a dependency description that was null. Plugin was not loaded.");
                        throw new PluginDependencyNotFound($"{plugin.Name} had a dependency description that was null. Plugin was not loaded.");
                    }

                    var found = false;
                    foreach (var assembly in this.ActiveAssemblies)
                    {
                        if (dep.Description.IsValid(assembly))
                        {
                            found = true;
                            break;
                        }
                    }

                    if (found == false && dep.Required == true)
                    {
                        notFound = dep.Description;
                        allDependenciesMet = false;
                        break;
                    }
                    else if (found == false && dep.Required == false)
                    {
                        _log.LogDebug($"{plugin.Name} had a dependency for {dep.Description.Name} that was not found in the Active Assemblies. The plugin was loaded anyway.");
                    }
                }

                if (!allDependenciesMet)
                {
                    _log.LogError(new EventId(), $"{plugin.Name} had a required dependency for {notFound.Name} that was not found in the Active Assemblies.");
                    throw new PluginDependencyNotFound($"{plugin.Name} had a required dependency for {notFound.Name} that was not found in the Active Assemblies.");
                }
            }

            return true;
        }

        private void RegisterAssemblyInPartManager(IPlugin plugin)
        {

            EnsureAllDependenciesMet(plugin);

            var pluginAssemblies = AvailablePluginAssemblies.FirstOrDefault(a => a.Item1.AssemblyName == plugin.AssemblyName)?.Item2;
            if (pluginAssemblies == null)
                throw new KeyNotFoundException($"Can not find assembles for plugin: {plugin.Name}");

            foreach (var assembly in pluginAssemblies)
            {
                _appPartManager.ApplicationParts.Add(new AssemblyPart(assembly));
            }
        }

        private void RemoveFromAssemblyInPartManager(IPlugin plugin)
        {
            var pluginAssemblies = AvailablePluginAssemblies.FirstOrDefault(a => a.Item1.AssemblyName == plugin.AssemblyName)?.Item2;
            if (pluginAssemblies == null)
                throw new KeyNotFoundException($"Can not find assembles for plugin: {plugin.Name}");

            foreach (var assembly in pluginAssemblies)
            {
                var assemblyPart = new AssemblyPart(assembly);
                var assmblyPartFromMgr = _appPartManager.ApplicationParts.FirstOrDefault(a => a.Name == assemblyPart.Name);

                if (assmblyPartFromMgr == null)
                    throw new NullReferenceException($"Can not find the assembly {assembly.FullName} in ApplicationPartManager");

                _appPartManager.ApplicationParts.Remove(assmblyPartFromMgr);
            }
        }

    }
}
