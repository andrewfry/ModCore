using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ModCore.Abstraction.Plugins
{
    public interface IPluginManager
    {
        IList<IPlugin> GetAvailablePlugins();

        IList<IPlugin> GetInstalledPlugins();

        IList<IPlugin> GetActivePlugins();

        void SetPluginDirPath(string pluginDir);

        IList<Assembly> GetAvailablePluginAssemblies();

        IList<Assembly> GetInstalledPluginAssemblies();

        IList<Assembly> GetActivePluginAssemblies();


    }
}
