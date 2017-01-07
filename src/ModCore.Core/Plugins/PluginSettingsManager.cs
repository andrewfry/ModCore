using ModCore.Abstraction.DataAccess;
using ModCore.Abstraction.Plugins;
using ModCore.Abstraction.Site;
using ModCore.Core.Site;
using ModCore.Models.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModCore.Core.Plugins
{
    public class PluginSettingsManager : IPluginSettingsManager
    {
        private IPlugin _plugin;
        private SettingsManager _settingManager;
        private readonly IDataRepositoryAsync<SiteSetting> _repository;

        public PluginSettingsManager(IDataRepositoryAsync<SiteSetting> repository)
        {
            _repository = repository;
        }

        public void SetPlugin(IPlugin plugin)
        {
            _plugin = plugin;
            var settingName = _plugin.AssemblyName.ToUpper() + "_" + _plugin.Name.ToUpper();
            _settingManager = new SettingsManager(_repository, settingName);
        }

        public async Task UpsertSettingAsync(string key, string regionName, object value)
        {
            await _settingManager.UpsertSettingAsync(key, regionName, value);
        }

        public async Task UpsertSettingAsync(SettingRegionPair pair, object value)
        {
            await _settingManager.UpsertSettingAsync(pair, value);
        }

        public async Task<T> GetSettingAsync<T>(string key)
        {
            return await _settingManager.GetSettingAsync<T>(key);
        }

        public async Task<object> GetSettingAsync(string key, string regionName)
        {
            return await _settingManager.GetSettingAsync(key, regionName);
        }

        public async Task<object> GetSettingAsync(string key)
        {
            return await _settingManager.GetSettingAsync(key);
        }

        public async Task<object> GetSettingAsync(SettingRegionPair pair)
        {
            return await _settingManager.GetSettingAsync(pair);
        }

        public async Task<T> GetSettingAsync<T>(string key, string regionName)
        {
            return await _settingManager.GetSettingAsync<T>(key, regionName);
        }

        public async Task<T> GetSettingAsync<T>(SettingRegionPair pair)
        {
            return await _settingManager.GetSettingAsync<T>(pair);
        }

        public async Task<bool> ContainsSettingAsync(SettingRegionPair pair)
        {
            return await _settingManager.ContainsSettingAsync(pair);
        }

        public async Task EnsureDefaultSettingAsync(SettingRegionPair pair, object value)
        {
            await _settingManager.EnsureDefaultSettingAsync(pair, value);
        }

        public async Task<List<SettingDescriptor>> GetAllAsync()
        {
            return await _settingManager.GetAllAsync();
        }

        public SettingRegionPair GetSettingRegionPair(string rawKey)
        {
            return _settingManager.GetSettingRegionPair(rawKey);
        }

    }
}
