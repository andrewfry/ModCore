using ModCore.Models.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModCore.Abstraction.Site
{
    public interface ISiteSettingsManagerAsync
    { 
        Task UpsertSettingAsync(string key, string regionName, object value);

        Task UpsertSettingAsync(SettingRegionPair pair, object value);

        Task<T> GetSettingAsync<T>(string key);

        Task<object> GetSettingAsync(string key, string regionName);

        Task<object> GetSettingAsync(string key);

        Task<object> GetSettingAsync(SettingRegionPair pair);

        Task<T> GetSettingAsync<T>(string key, string regionName);

        Task<T> GetSettingAsync<T>(SettingRegionPair pair);
    }
}
