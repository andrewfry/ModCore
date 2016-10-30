using ModCore.Abstraction.Themes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModCore.Core.Themes
{
    public class Theme : ITheme
    {
        public Theme() {

        }

        public string ThemeName { get; set; }

        public string ThemeVersion { get; set; }

        public string Description { get; set; }

        public string DisplayName { get; set; }

        public string PreviewURL { get; set; }
    }
}
