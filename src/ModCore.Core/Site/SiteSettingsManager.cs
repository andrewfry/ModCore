using ModCore.Abstraction.DataAccess;
using ModCore.Abstraction.Site;
using ModCore.Models.Core;
using ModCore.Specifications.Site;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModCore.Core.Site
{
    public class SiteSettingsManager : ISiteSettingsManagerAsync
    {

        private readonly IDataRepositoryAsync<SiteSetting> _repository;
        private readonly SiteSetting _settings;
        private const string settingName = "MAIN_SITE_SETTINGS";

        public SiteSettingsManager(IDataRepositoryAsync<SiteSetting> repository)
        {
            _repository = repository;

            var siteSetting = repository.FindAsync(new GetSettingByName(settingName)).Result;
            if (siteSetting == null)
            {
                siteSetting = new SiteSetting()
                {
                    Name = settingName
                };

                repository.InsertAsync(siteSetting);
            }

            _settings = siteSetting;
        }

        public async Task UpsertSettingAsync(string key, string regionName, object value)
        {
            await UpsertSettingAsync(new SettingRegionPair(regionName, key), value);
        }

        public async Task UpsertSettingAsync(SettingRegionPair pair, object value)
        {
            var key = string.Concat(pair.Region, "_", pair.Key);
            var nameOfType = value.GetType().FullName;
            var settingExists = _settings.Settings.ContainsKey(key.ToLower());
            SettingValue setting = null;

            if (settingExists)
            {
                setting = _settings.Settings[key.ToLower()];
                setting.Value = value;
                setting.TypeName = nameOfType;

                _settings.Settings.Add(key, setting);
            }
            else
            {
                setting = new SettingValue()
                {
                    TypeName = nameOfType,
                    Value = value,
                };
            }

            await _repository.UpdateAsync(_settings);
        }

        public async Task<object> GetSettingAsync(string key, string regionName)
        {
            return await GetSettingAsync(new SettingRegionPair(key, regionName));
        }

        public async Task<object> GetSettingAsync(SettingRegionPair pair)
        {
            var key = string.Concat(pair.Region, "_", pair.Key);
            return await GetSettingAsync(key);
        }

        public async Task<object> GetSettingAsync(string key)
        {
            return await GetSettingValueAsync(key);
        }

        public async Task<T> GetSettingAsync<T>(string key, string regionName)
        {
            return await GetSettingAsync<T>(new SettingRegionPair(key, regionName));
        }

        public async Task<T> GetSettingAsync<T>(SettingRegionPair pair)
        {
            var key = string.Concat(pair.Region, "_", pair.Key);
            return await GetSettingAsync<T>(key);
        }


        public async Task<bool> ContainsSettingAsync(SettingRegionPair pair)
        {
            var key = string.Concat(pair.Region, "_", pair.Key);
            var exists = _settings.Settings.ContainsKey(key.ToLower());

            return exists;
        }

        public async Task EnsureDefaultSettingAsync(SettingRegionPair pair, object value)
        {
            var exists = await ContainsSettingAsync(pair);
            if (exists)
                return;

            await UpsertSettingAsync(pair, value);
        }

        public async Task<T> GetSettingAsync<T>(string key)
        {
            var setting = await GetSettingValueAsync(key);
            var nameOfType = setting.GetType().FullName;

            if (setting.TypeName != nameOfType)
            {
                throw new Exception($"The Key: {key} expected a type of {nameOfType} but got a type of {setting.TypeName}.");
            }

            return (T)setting.Value;
        }

        private async Task<SettingValue> GetSettingValueAsync(string key)
        {
            var exists = _settings.Settings.ContainsKey(key.ToLower());

            if (exists == false)
            {
                throw new KeyNotFoundException(key);
            }

            return _settings.Settings[key.ToLower()];
        }

    }



    public static class BuiltInSettings
    {
        public static SettingRegionPair LogLevel => new SettingRegionPair("LOG", "LEVEL");

        public static SettingRegionPair AuthenticationLockOut => new SettingRegionPair("AUTHENTICATION", "LOCK_OUT");

    }
}
