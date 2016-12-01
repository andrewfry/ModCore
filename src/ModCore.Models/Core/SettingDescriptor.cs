using ModCore.Models.BaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModCore.Models.Core
{
    public class SettingDescriptor
    {
        public string Key { get; set; }

        public string RegionName { get; set; }

        public object Value { get; set; }

        public string TypeName { get; set; }
    }
   
}
