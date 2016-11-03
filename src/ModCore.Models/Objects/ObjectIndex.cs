using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModCore.Models.Objects
{
    public class ObjectIndex
    {
        public HashSet<string> FieldNames { get; set; }

        public ObjectIndex()
        {
            this.FieldNames = new HashSet<string>();
        }
    }
}
