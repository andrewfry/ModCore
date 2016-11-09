using ModCore.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ModCore.Models.Themes;

namespace ModCore.ViewModels.Theme
{
    public class vTheme
    {
        public bool Active { get; set; }

        public string ThemeName { get; set; }

        public string ThemeVersion { get; set; }

        public string Description { get; set; }

        public string DisplayName { get; set; }

        public string CSSLocation { get; set; }
    }

    public class vThemeList
    {
        public List<vTheme> ThemeList { get; set; }
    }

    public static partial class Extension
    {
        public static vTheme ToViewModel(this SiteTheme theme)
        {
            return new vTheme
            {
                Description = theme.Description,
                DisplayName = theme.DisplayName,
                ThemeName = theme.ThemeName,
                ThemeVersion = theme.ThemeVersion,
                Active = theme.Active
            };

        }

    }
}
