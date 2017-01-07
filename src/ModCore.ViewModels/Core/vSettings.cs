using ModCore.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModCore.ViewModels.Core
{
    public class vSettings : BaseViewModel
    {
        public List<vSettingValue> Settings { get; set; }

        public string AssemblyName { get; set; }

        public string Name { get; set; }
    }
}
