using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ModCore.Models.BaseEntities;


namespace ModCore.Models.Themes
{
    public class SiteTheme : BaseEntity
    {
        public bool Active { get; set; }

        public string ThemeName { get; set; }

        public string ThemeVersion { get; set; }

        public string Description { get; set; }

        public string DisplayName { get; set; }
    }

}
