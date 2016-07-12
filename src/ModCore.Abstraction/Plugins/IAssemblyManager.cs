using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyModel;

namespace ModCore.Abstraction.Plugins
{
    public interface IAssemblyManager
    {
        IEnumerable<Assembly> GetAssemblies(string path);

        IEnumerable<Assembly> CoreApplicationAssemblies { get; }

        IEnumerable<Dependency> ApplicationDependencies { get; }

        IEnumerable<Assembly> GetReferencingAssemblies(string assemblyNameStartsWith);
    }
}
