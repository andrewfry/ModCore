using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModCore.Models.Objects
{
    public class ObjectProperty
    {
        public string Name { get; set; }

        public PropertyType Type { get; set; }

        public List<ObjectPropertyValidation> Validation { get; set; }
    }
}
