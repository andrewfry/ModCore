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
        private readonly SettingsManager _settingManager;
        private const string settingName = "MAIN_SITE_SETTINGS";

        public SiteSettingsManager(IDataRepositoryAsync<SiteSetting> repository)
        {
            _repository = repository;
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



    public static class BuiltInSettings
    {
        public static SettingRegionPair LogLevel => new SettingRegionPair("LOG", "LEVEL");

        public static SettingRegionPair AuthenticationLockOut => new SettingRegionPair("AUTHENTICATION", "LOCK_OUT");

        public static SettingRegionPair SessionId => new SettingRegionPair("GENERAL", "SESSION_ID");

        public static SettingRegionPair SessionTimeOut => new SettingRegionPair("GENERAL", "SESSION_TIMEOUT");

        public static SettingRegionPair UsrActTrking => new SettingRegionPair("USER", "ACTIVITY_TRACKING");

        public static SettingRegionPair UsrActTrkingDetailed => new SettingRegionPair("USER", "ACTIVITY_TRACKING_DETAILED");

        public static SettingRegionPair UsrActTrkingBaseModelRecord => new SettingRegionPair("USER", "ACTIVITY_TRACKING_BASEMODEL");

    }
}
