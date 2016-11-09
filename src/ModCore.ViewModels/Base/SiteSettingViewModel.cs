using ModCore.ViewModels.Theme;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModCore.ViewModels.Base
{
    public class SiteSettingViewModel
    {
        public vTheme Theme { get; set; }

        public SiteSettingViewModel()
        {
            Theme = new vTheme();
        }
    }

}
