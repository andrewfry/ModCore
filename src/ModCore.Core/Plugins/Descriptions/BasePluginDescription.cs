using ModCore.Abstraction.Plugins.Builtins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ModCore.Core.Plugins.Descriptions
{
    public abstract class BasePluginDescription : IPluginDescription
    {

        public abstract string Name { get; }

        public abstract List<Type> RequiredInterfaces { get; }

        public virtual bool IsValid(Assembly assembly)
        {
            var types = assembly.DefinedTypes.Select(a => a.GetType());

            foreach (var iface in RequiredInterfaces)
            {
                if (types.Any(a => a.IsAssignableFrom(iface)) == false)
                {
                    return false;
                }
                else
                {
                    throw new Exception($"The assembly {assembly.FullName} does not contain a concrete type for {iface.GetType().FullName}");
                }
            }

            return true;
        }
    }

}
