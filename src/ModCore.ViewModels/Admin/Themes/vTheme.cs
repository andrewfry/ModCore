using ModCore.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModCore.ViewModels.Admin.Themes
{
    public class vTheme
    {
        public string ThemeName { get; set; }

        public string ThemeVersion { get; set; }

        public string Description { get; set; }

        public string DisplayName { get; set; }
    }

    public class vThemeList
    {
        public List<vTheme> ThemeList { get; set; }
    }
}
