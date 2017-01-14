using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ModCore.Abstraction.Plugins.Builtins
{
    public interface IPluginDescription
    {
        string Name { get; }

        List<Type> RequiredInterfaces { get; }

        bool IsValid(Assembly assembly);
    }
}
