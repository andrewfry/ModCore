using ModCore.Abstraction.Site;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModCore.Core.Site
{
    public class LayoutManager : ILayoutManager
    {
        public  string DefaultLayoutPath
        {
            get
            {
                return "/Views/Shared/_layout.cshtml";
            }
        }
    }
}
