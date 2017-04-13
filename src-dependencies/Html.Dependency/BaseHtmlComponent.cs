using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModHtml.Dependency
{
    public class BaseHtmlComponent : IHtmlComponent
    {
        public string Name { get; set; }

        public string BaseClass { get; set; }

        public int Position { get; set; }

        public string DisplayTypeName { get; set; } 

    }
}
