using ModCore.Models.BaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pages.Plugin.Models
{
    public class Page : BaseEntity
    {
        public string PageName { get; set; }

        public string Url { get; set; }

        public string HTMLContent { get; set; }

        public PageStatus PageStatus { get; set; }

    }
}
