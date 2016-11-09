using ModCore.Models.BaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModCore.Models.Page
{
    public class Page : BaseEntity
    {
        public string PageName { get; set; }

        public string FriendlyURL { get; set; }

        public string HTMLContent { get; set; }

        public bool Active { get; set; }

    }
}
