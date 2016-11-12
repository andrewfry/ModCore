using ModCore.Abstraction.Site;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModCore.Core.Site
{
    public class SiteSettingsManager : ISiteSettingsManager
    {
        public SiteSettingsManager()
        {
                
        }

        public bool AddSettings(string key, object value)
        {
            throw new NotImplementedException();
        }

        public T GetSetting<T>(string key)
        {
            throw new NotImplementedException();
        }

    }
}
