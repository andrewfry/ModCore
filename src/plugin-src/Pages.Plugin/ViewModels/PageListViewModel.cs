using ModCore.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pages.Plugin.ViewModels
{
    public class PageListViewModel : BaseViewModel
    {
        public ICollection<PageViewModel> PageList { get; set; }
    }
}
