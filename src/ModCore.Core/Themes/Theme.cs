using ModCore.Abstraction.Themes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModCore.Core.Themes
{
    public class Theme : ITheme
    {
        private string _themeName;
        private string _themeDescription;
        private string _displayName;
        private string _themeVersion;
        private string _cssLocation;

        public string ThemeName
        {
            get { return _themeName; }
            set { _themeName = value; }
        }

        public string Description
        {
            get { return _themeDescription; }
            set { _themeDescription = value; }
        }

        public string DisplayName
        {
            get { return _displayName; }
            set { _displayName = value; }
        }

        public string ThemeVersion
        {
            get { return _themeVersion; }
            set { _themeVersion = value; }
        }

    }
}
