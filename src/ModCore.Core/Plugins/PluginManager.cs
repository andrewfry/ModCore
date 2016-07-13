using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using ModCore.Abstraction.DataAccess;
using ModCore.Abstraction.Plugins;
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
            IHostingEnvironment hostingEnvironment, IDataRepository<InstalledPlugin> repository)
        {
            _assemblyManager = assemblyManager;
            _configurationRoot = configurationRoot;
            _hostingEnvironment = hostingEnvironment;

            _repository = repository;

            var pluginDir = _configurationRoot.GetValue<string>("PluginDir");
            _pluginDirPath = Path.Combine(_hostingEnvironment.ContentRootPath, pluginDir);

            if (string.IsNullOrEmpty(_pluginDirPath))
            {
                throw new ArgumentNullException("pluginDir");
            }
        }



        //private IDictionary<IPlugin, Assembly> GetPluginAssemblyFromDir()
        //{
        //    if (_plugins == null)
        //    {
        //        _plugins = new Dictionary<IPlugin, Assembly>();

        //        foreach (var dll in GetAllAssembliesFromDir().Where(a => a.FullName.ToLower().Contains(".plugin")))
        //        {
        //            var plugin = dll.GetTypes().SingleOrDefault(t => typeof(IPlugin).IsAssignableFrom(t) &&
        //                    typeof(BasePlugin).IsAssignableFrom(t));

        //            if (plugin != null)
        //            {
        //                IPlugin addIn = (IPlugin)Activator.CreateInstance(plugin);
        //                _plugins.Add(addIn, dll);
        //            }
        //        }

        //    }

        //    return _plugins;
        //}



    }
}
