using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModCore.Models.Core
{
    public class SettingRegionPair
    {
        public SettingRegionPair(string region, string key)
        {
            Key = key;
            Region = region;
        }

        public string Key { get; set; }

        public string Region { get; set; }
    }
}
