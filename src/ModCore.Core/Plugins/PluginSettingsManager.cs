using ModCore.Abstraction.Plugins;
using ModCore.Abstraction.Site;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModCore.Core.Plugins
{
    public class PluginSettingsManager : IPluginSettingsManager
    {
        public bool AddSettings(string key, object value, IPlugin plugin)
        {
            throw new NotImplementedException();
        }

        public T GetSetting<T>(string key, IPlugin plugin)
        {
            throw new NotImplementedException();
        }

    }
}
