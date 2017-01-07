using ModCore.Abstraction.DataAccess;
using ModCore.Abstraction.Site;
using ModCore.Models.Core;
using ModCore.Specifications.Site;
using ModCore.Utilities.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModCore.Core.Site
{
    public class SettingsManager
    {

        private readonly IDataRepositoryAsync<SiteSetting> _repository;
        private readonly SiteSetting _settings;

        public SettingsManager(IDataRepositoryAsync<SiteSetting> repository, string settingName)
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
            var key = GenerateKey(pair.Region, pair.Key);
            var nameOfType = value.GetType().FullName;
            var qualifiedNameOfType = value.GetType().AssemblyQualifiedName;
            var settingExists = _settings.Settings.ContainsKey(key.ToUpper());
            SettingValue setting = null;

            if (settingExists)
            {
                setting = _settings.Settings[key.ToUpper()];
                //setting.Value = Convert.ChangeType(value, Type.GetType(setting.AssemblyQualifiedTypeName));
                var curType = Type.GetType(setting.AssemblyQualifiedTypeName);
                setting.Value = value.ChangeTypeWithEnumConversion(curType);
            }
            else
            {
                setting = new SettingValue()
                {
                    TypeName = nameOfType,
                    AssemblyQualifiedTypeName = qualifiedNameOfType,
                    Value = value,
                };

                _settings.Settings.Add(key, setting);
            }

            await _repository.UpdateAsync(_settings);
        }

        public async Task<object> GetSettingAsync(string key, string regionName)
        {
            return await GetSettingAsync(new SettingRegionPair(key, regionName));
        }

        public async Task<object> GetSettingAsync(SettingRegionPair pair)
        {
            var key = GenerateKey(pair.Region, pair.Key);
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
            var key = GenerateKey(pair.Region, pair.Key);
            return await GetSettingAsync<T>(key);
        }
        
        public async Task<bool> ContainsSettingAsync(SettingRegionPair pair)
        {
            var key = GenerateKey(pair.Region, pair.Key);
            var exists = _settings.Settings.ContainsKey(key.ToUpper());

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
            var nameOfType = setting.Value.GetType().FullName;

            if (setting.TypeName != nameOfType)
            {
                throw new Exception($"The Key: {key} expected a type of {nameOfType} but got a type of {setting.TypeName}.");
            }

            return (T)setting.Value;
        }

        public async Task<List<SettingDescriptor>> GetAllAsync()
        {
            var returnValue = new List<SettingDescriptor>();

            foreach (var setting in _settings.Settings)
            {
                returnValue.Add(GetSettingDescriptor(setting));
            }

            return returnValue;
        }

        public SettingRegionPair GetSettingRegionPair(string rawKey)
        {
            var split = rawKey.Split('|');
            var region = split.Length > 1 ? split[0] : "";
            var key = split.Length > 1 ? split[1] : split[0];

            return new SettingRegionPair(region, key);
        }

        private string GenerateKey(string region, string key)
        {
            key = key.Replace("|", "");

            return string.Concat(region, "|", key).ToUpper();
        }

        private SettingDescriptor GetSettingDescriptor(KeyValuePair<string, SettingValue> setting)
        {
            var settingPair = GetSettingRegionPair(setting.Key);

            return new SettingDescriptor
            {
                Key = settingPair.Key,
                RegionName = settingPair.Region,
                Value = setting.Value.Value,
                TypeName = setting.Value.TypeName,
                 AssemblyQualifiedTypeName = setting.Value.AssemblyQualifiedTypeName
            };
        }

        private async Task<SettingValue> GetSettingValueAsync(string key)
        {
            var exists = _settings.Settings.ContainsKey(key.ToUpper());

            if (exists == false)
            {
                throw new KeyNotFoundException(key);
            }

            return _settings.Settings[key.ToUpper()];
        }



    }

}
