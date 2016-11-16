using ModCore.Models.BaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModCore.Models.Core
{
    public class Setting : BaseEntity
    {
        public string Name { get; set; }

        public Dictionary<string, SettingValue> Settings { get; set; }

        public Setting()
        {
            this.Settings = new Dictionary<string, SettingValue>();
        }
    }


    public class SettingValue
    {
        public object Value { get; set; }

        public string TypeName { get; set; }
    }
}
