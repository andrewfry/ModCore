using ModCore.ViewModels.Base;
using Pages.Plugin.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Pages.Plugin.ViewModels
{
    public class PageViewModel : BaseViewModel
    {
        public string Id { get; set; }

        [Display(Name ="Page Name")]
        [Required]
        public string PageName { get; set; }

        [Required]
        [Display(Name = "Friendly Url")]
        public string Url { get; set; }

        [Required]
        [Display(Name = "Page Content")]
        public string HTMLContent { get; set; }

        public PageStatus PageStatus { get; set; }
    }
}
