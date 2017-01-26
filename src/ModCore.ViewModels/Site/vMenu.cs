using ModCore.Models.Site;
using ModCore.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModCore.ViewModels.Site
{
    public class vMenu : BaseViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public List<MenuItem> MenuItems { get; set; }

    }
}
