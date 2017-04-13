using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModHtml.Dependency
{
    public interface IHtmlComponent
    {
        string Name { get;  }

        string BaseClass { get; }

        int Position { get; }

        string DisplayTypeName { get;}
    }
}
