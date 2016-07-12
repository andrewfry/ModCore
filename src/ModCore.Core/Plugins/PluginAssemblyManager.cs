using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyModel;
using ModCore.Abstraction.Plugins;
using System.Runtime.Loader;
using Microsoft.Extensions.PlatformAbstractions;

namespace ModCore.Core.Plugins
{
    public class PluginAssemblyManager : IAssemblyManager
    {
        internal static HashSet<string> ReferenceAssembliesStartsWith { get; } = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "ModCore"
        };

        private List<Assembly> _coreApplicationAssemblies;
        private List<Dependency> _dependencies;

        public IEnumerable<Assembly> CoreApplicationAssemblies
        {
            get
            {
                if (_coreApplicationAssemblies == null)
                {
                    _coreApplicationAssemblies = new List<Assembly>();

                    foreach (var assemblyStart in ReferenceAssembliesStartsWith)
                    {
                        _coreApplicationAssemblies.AddRange(GetReferencingAssemblies(assemblyStart));
                    }
                }

                return _coreApplicationAssemblies;
            }
        }

        public IEnumerable<Dependency> ApplicationDependencies
        {
            get
            {
                if (_dependencies == null)
                {
                    _dependencies = new List<Dependency>();

                    foreach (var assemblyStart in ReferenceAssembliesStartsWith)
                    {
                        _dependencies.AddRange(GetDependencies(assemblyStart));
                    }
                }

                return _dependencies;
            }
        }

        public PluginAssemblyManager()
        {

        }

        public IEnumerable<Assembly> GetReferencingAssemblies(string assemblyNameStartsWith)
        {
            var assemblies = new List<Assembly>();
            var dependencies = DependencyContext.Default.RuntimeLibraries;
            foreach (var library in dependencies)
            {
                if (IsCandidateLibrary(library, assemblyNameStartsWith))
                {
                    var assembly = Assembly.Load(new AssemblyName(library.Name));
                    assemblies.Add(assembly);

                }
            }
            return assemblies;
        }

        private IEnumerable<Dependency> GetDependencies(string assemblyStartsWith)
        {
            var dependencies = new HashSet<Dependency>();

            var depcontext = DependencyContext.Default.RuntimeLibraries;
            foreach (var library in depcontext)
            {
                if (IsCandidateLibrary(library, assemblyStartsWith))
                {
                    foreach(var dep in library.Dependencies)
                    {
                        dependencies.Add(dep);
                    }
                }
            }

            return dependencies;
        }

        private bool IsCandidateLibrary(RuntimeLibrary library, string assemblyName)
        {
            return library.Name.StartsWith(assemblyName)
                || library.Dependencies.Any(d => d.Name.StartsWith(assemblyName));
        }

        private bool AlreadyLoaded(AssemblyName assemblyName)
        {
            return CoreApplicationAssemblies.Any(a => a.GetName().Name == assemblyName.Name)
                || ApplicationDependencies.Any(a=>a.Name == assemblyName.Name);
        }

        public IEnumerable<Assembly> GetAssemblies(string path)
        {

            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException("path");

            if (!Directory.Exists(path))
                throw new DirectoryNotFoundException(path);

            var assemblies = new List<Assembly>();

            foreach (var folder in Directory.EnumerateDirectories(path))
            {
                var dllsToAdd = Directory.EnumerateFiles(folder, "*.dll");
                foreach (string dllPath in dllsToAdd)
                {
                    var assemblyName = FromPath(dllPath);

                    if (!AlreadyLoaded(assemblyName))
                    {
                        Assembly assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(dllPath);
                        assemblies.Add(assembly);
                    }
                }
            }

            return assemblies;
        }

        private AssemblyName FromPath(string dllPath)
        {
            var pieces = dllPath.Split('\\').ToArray();
            var dllName = pieces[pieces.Length - 1];
            var assemblyName = dllName.Substring(0, dllName.Length-4);
            return new AssemblyName(assemblyName);
        }

        public IList<Assembly> GetAssembliesWithPlugin(string path)
        {
            return this.GetAssemblies(path)
                    .Where(a => a.GetTypes().Any(type =>
                         typeof(IPlugin).GetTypeInfo().IsAssignableFrom(type)
                     && typeof(BasePlugin).GetTypeInfo().IsAssignableFrom(type)
                     && type.GetTypeInfo().IsClass
                    ))
                    .ToList();
        }

        //private static bool IsCandidateAssembly(Assembly assembly)
        //{
        //    if (assembly.FullName.ToLower().Contains("modcore"))
        //        return false;

        //    return true;
        //}

        //private static bool IsCandidateCompilationLibrary(CompilationLibrary compilationLibrary)
        //{
        //    if (compilationLibrary.Name.ToLower().Contains("modcore"))
        //        return false;

        //    if (!compilationLibrary.Dependencies.Any(d => d.Name.ToLower().Contains("modcore")))
        //        return false;

        //    return true;
        //}
    }
}
