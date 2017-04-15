using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModHtml.Dependency.ModelTemplates
{
    public class PageTemplate
    {
        public string TemplateName { get; set; }

        public IList<PageTemplateSection> TemplateSections { get; set; }
    }
}
