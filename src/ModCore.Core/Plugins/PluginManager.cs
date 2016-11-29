using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ModCore.Abstraction.DataAccess;
using ModCore.Abstraction.Plugins;
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
            ApplicationPartManager appPartManager)

        {
            _assemblyManager = assemblyManager;
            _configurationRoot = configurationRoot;
            _hostingEnvironment = hostingEnvironment;
            _appPartManager = appPartManager;

            _repository = repository;

            var pluginDir = _configurationRoot.GetValue<string>("PluginDir");
            _pluginDirPath = Path.Combine(_hostingEnvironment.ContentRootPath, pluginDir);

            if (string.IsNullOrEmpty(_pluginDirPath))
            {
                throw new ArgumentNullException("pluginDir");
            }
        }

        public void ActivatePlugin(IPlugin plugin)
        {
            var installedPlugin = _repository.Find(new InstalledPluginForPlugin(plugin));
            if (installedPlugin == null)
                throw new NullReferenceException($"Can not find installed plugin for {plugin.Name}");

            installedPlugin.Active = true;
            installedPlugin.DateActivated = DateTime.UtcNow;
            _repository.Update(installedPlugin);

            RegisterAssemblyInPartManager(plugin);

            Refresh();
        }

        public void DeactivatePlugin(IPlugin plugin)
        {
            var installedPlugin = _repository.Find(new InstalledPluginForPlugin(plugin));
            if (installedPlugin == null)
                throw new NullReferenceException($"Can not find installed plugin for {plugin.Name}");

            installedPlugin.Active = false;
            installedPlugin.DateDeactivated = DateTime.UtcNow;
            _repository.Update(installedPlugin);

            RemoveFromAssemblyInPartManager(plugin);

            Refresh();
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
                coreBuilder.Services.Remove(srv);
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
        
        private void RegisterAssemblyInPartManager(IPlugin plugin)
        {
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
