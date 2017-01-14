using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModCore.Abstraction.Plugins.Builtins
{
    public interface IPluginDependency
    {

        bool Required { get; }

        IPluginDescription Description { get; }
    }
}
