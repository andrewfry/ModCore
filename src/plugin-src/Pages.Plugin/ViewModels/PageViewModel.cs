using ModCore.ViewModels.Base;
using ModHtml.Dependency;
using ModHtml.Dependency.ModelTemplates;
using Pages.Plugin.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace Pages.Plugin.ViewModels
{
    public class PageViewModel : BaseViewModel
    {
        public PageViewModel()
        {
            HtmlComponents = new List<IHtmlComponent>();
        }

        public string Id { get; set; }

        [Display(Name ="Page Name")]
        [Required]
        public string PageName { get; set; }

        [Required]
        [Display(Name = "Friendly Url")]
        public string Url { get; set; }

        //[Required]
        //[Display(Name = "Page Content")]
        //public string HTMLContent { get; set; }

        public IList<IHtmlComponent> HtmlComponents { get; set; }

        public IList<IHtmlComponent> AvailableHtmlComponents { get; set; }

        public IList<PageTemplate> AvailableTemplates { get; set; }

        public PageTemplate PageTemplate { get; set; }

        public PageStatus PageStatus { get; set; }


    }
}
