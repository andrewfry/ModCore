using Microsoft.AspNetCore.Routing;
using ModCore.Abstraction.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ModCore.Core.Plugins
{
    public class PluginManager : IPluginManager
    {
        private string _pluginDirPath;
        private IDictionary<IPlugin, Assembly> _plugins;
        private IAssemblyManager _assemblyManager;

        public PluginManager(IAssemblyManager assemblyManager)
        {
            _assemblyManager = assemblyManager;

        }

        private IList<Assembly> GetPluginsFromDir()
        {
            if (string.IsNullOrEmpty(_pluginDirPath))
            {
                throw new ArgumentNullException("pluginDir");
            }

            return _assemblyManager.GetAssemblies(_pluginDirPath).ToList();

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
            var plugins = new List<IPlugin>();

            foreach (var dll in GetActivePluginAssemblies().Where(a => a.FullName.ToLower().Contains(".plugin")))
            {
                var plugin = dll.GetTypes().SingleOrDefault(t => typeof(IPlugin).IsAssignableFrom(t) &&
                        typeof(BasePlugin).IsAssignableFrom(t));

                if (plugin != null)
                {
                    IPlugin addIn = (IPlugin)Activator.CreateInstance(plugin);
                    plugins.Add(addIn);
                }
            }

            return plugins;
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
            return this.GetPluginsFromDir();
        }

    }
}
