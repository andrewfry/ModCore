using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ModHtml.Dependency;
using ModHtml.Dependency.HtmlComponentTypes;

namespace Themes.Plugin.Models
{
    public class PageTemplateSection
    {
        public int Position { get; set; }

        public string CssClass { get; set; }

        public string SectionName { get; set; }

        public List<IHtmlComponent> HtmlComponents { get; set; }

        public PageTemplateSection()
        {
            HtmlComponents = new List<IHtmlComponent>();
        }

    }


}
