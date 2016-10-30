using ModCore.Models.BaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModCore.Models.Communication
{
    public class EmailLayout : BaseEntity
    {
        public string Name { get; set; }

        public string Html { get; set; }
    }
}
