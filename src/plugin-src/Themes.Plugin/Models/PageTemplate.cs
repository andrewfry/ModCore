using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Themes.Plugin.Models
{
    public class PageTemplate
    {
        public string TemplateName { get; set; }

        public List<PageTemplateSection> TemplateSections { get; set; }
    }
}
