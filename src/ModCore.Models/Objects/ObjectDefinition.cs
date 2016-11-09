using ModCore.Models.BaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModCore.Models.Objects
{
    public class ObjectDefinition : BaseEntity
    {

        public string CollectionName { get; set; }

        public string ObjectName { get; set; }

        public string DisplayName { get; set; }

        public bool Inherits { get; set; }

        public string InheritedName { get; set; }

        public List<ObjectProperty> Properties { get; set; }

        public List<ObjectIndex> Indexes { get; set; }

        public ObjectDefinition()
        {
            this.Properties = new List<ObjectProperty>();
            this.Indexes = new List<ObjectIndex>();
        }

    }
}
