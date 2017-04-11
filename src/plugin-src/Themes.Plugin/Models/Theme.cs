using ModCore.Models.BaseEntities;
using ModHtml.Dependency.ModelTemplates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Themes.Plugin.Models
{
    public class Theme : BaseEntity
    {
        public string ThemeName { get; set; }

        public List<PageTemplate> PageTemplates { get; set; }

        public PageTemplate MasterPage { get; set; }

        public string CSS { get; set; }

        //list of js files or source?? bundle location etc, look into
    }
}
