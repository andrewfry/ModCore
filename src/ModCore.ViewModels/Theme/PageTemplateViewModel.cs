using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModCore.ViewModels.Theme
{
    public class PageTemplateViewModel
    {   

        public List<TemplateSectionViewModel> TemplateSections { get; set; }
    }

    public class TemplateSectionViewModel
    {
        public string SectionName { get; set; }

        public string SectionBaseClass { get; set; }

        public List<PageHTMLObjectViewModel> HTMLObjects { get; set; }
    }
    public class PageHTMLObjectViewModel
    {
        public string FieldName { get; set; }

        public int Position { get; set; }

        public string BaseClass { get; set; }

        public HTMLObjectType Type { get; set; }

        public string DisplayLabel { get; set; }

    }

    public enum HTMLObjectType {
        Textbox = 1
    }
}
