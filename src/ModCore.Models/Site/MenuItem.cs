using ModCore.Models.BaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModCore.Models.Site
{
    public class MenuItem 
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public int Order { get; set; }

        public bool isParent { get { return !Children.Any(); } }

        public List<MenuItem> Children { get; set; }

        public MenuItem()
        {
            Children = new List<MenuItem>();
        }

    }
}
