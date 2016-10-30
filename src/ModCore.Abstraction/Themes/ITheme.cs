using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModCore.Abstraction.Themes
{
    public interface ITheme
    {
        string ThemeName { get;}

        string ThemeVersion { get; }

        string Description { get; }

        string DisplayName { get;}

        string PreviewURL { get; set; }

    }
}
