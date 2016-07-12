using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyModel;
using ModCore.Abstraction.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ModCore.Core.Plugins
{
    public class PluginManager : IPluginManager
    {
        private static readonly Lazy<PluginManager> lazy = new Lazy<PluginManager>(() => new PluginManager());
        private string _pluginDirPath;
        private IDictionary<IPlugin, Assembly> _plugins;
        private IAssemblyManager _assemblyManager;
        private IList<Assembly> _allAssembliesInDir;

        public static PluginManager Instance { get { return lazy.Value; } }

        private PluginManager()
        {
            _assemblyManager = new PluginAssemblyManager();
        }

        private IList<Assembly> GetAllAssembliesFromDir()
        {
            if (_allAssembliesInDir == null)
            {
                _allAssembliesInDir = new List<Assembly>();

                if (string.IsNullOrEmpty(_pluginDirPath))
                {
                    throw new ArgumentNullException("pluginDir");
                }

                _allAssembliesInDir = _assemblyManager.GetAssemblies(_pluginDirPath).ToList();
            }

            return _allAssembliesInDir;
        }

        private IDictionary<IPlugin, Assembly> GetPluginAssemblyFromDir()
        {
            if (_plugins == null)
            {
                _plugins = new Dictionary<IPlugin, Assembly>();

                foreach (var dll in GetAllAssembliesFromDir().Where(a => a.FullName.ToLower().Contains(".plugin")))
                {
                    var plugin = dll.GetTypes().SingleOrDefault(t => typeof(IPlugin).IsAssignableFrom(t) &&
                            typeof(BasePlugin).IsAssignableFrom(t));

                    if (plugin != null)
                    {
                        IPlugin addIn = (IPlugin)Activator.CreateInstance(plugin);
                        _plugins.Add(addIn, dll);
                    }
                }

            }

            return _plugins;
        }

        public IList<IPlugin> GetAvailablePlugins()
        {
            throw new NotImplementedException();
        }

        public IList<IPlugin> GetInstalledPlugins()
        {
            throw new NotImplementedException();
        }

        public IList<IPlugin> GetActivePlugins()
        {

            return GetPluginAssemblyFromDir().Select(a => a.Key).ToList();
        }

        public void SetPluginDirPath(string pluginDir)
        {
            if (string.IsNullOrEmpty(pluginDir))
            {
                throw new ArgumentNullException("pluginDir");
            }

            _pluginDirPath = pluginDir;

        }

        public IList<Assembly> GetAvailablePluginAssemblies()
        {
            throw new NotImplementedException();
        }

        public IList<Assembly> GetInstalledPluginAssemblies()
        {
            throw new NotImplementedException();
        }

        public IList<Assembly> GetActivePluginAssemblies()
        {
            return GetPluginAssemblyFromDir().Select(a => a.Value).ToList();
        }

    }
}
