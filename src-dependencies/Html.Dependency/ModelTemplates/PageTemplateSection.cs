using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModHtml.Dependency.ModelTemplates
{
    public class PageTemplateSection
    {
        public int Position { get; set; }

        public string CssClass { get; set; }

        public string SectionName { get; set; }

        public IList<IHtmlComponent> HtmlComponents { get; set; }

        public PageTemplateSection()
        {
            HtmlComponents = new List<IHtmlComponent>();
        }

    }
}
