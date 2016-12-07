using ModCore.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Pages.Plugin.Models
{
    public class PageViewModel : BaseViewModel
    {

        public string PageName { get; set; }

        public string Url { get; set; }

        public string HTMLContent { get; set; }

        public bool Active { get; set; }
    }
}
