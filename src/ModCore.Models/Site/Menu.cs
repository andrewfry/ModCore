using ModCore.Models.BaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModCore.Models.Site
{
    public class Menu : BaseEntity
    {

        public string Name { get; set; }

        public List<MenuItem> MenuItems { get; set; }

    }
}
