using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModCore.Abstraction.Themes
{
    public interface IThemeManager
    {
        IList<ITheme> AvailableThemes { get; }

        ITheme ActiveTheme { get; }

        void ActivateTheme(ITheme theme);

        string ThemeDirectory { get; }
    }
}
