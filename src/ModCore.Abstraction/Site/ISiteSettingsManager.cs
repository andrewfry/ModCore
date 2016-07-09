using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModCore.Abstraction.Site
{
    public interface ISiteSettingsManager
    { 
        bool AddSettings(string key, object value);

        T GetSetting<T>(string key);

    }
}
