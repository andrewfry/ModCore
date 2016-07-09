using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyModel;
using ModCore.Abstraction.Plugins;
using System.Runtime.Loader;

namespace ModCore.Core.Plugins
{
    public class PluginAssemblyManager : IAssemblyManager
    {
        public IEnumerable<Assembly> GetAssemblies(string path)
        {

            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException("path");

            if (!Directory.Exists(path))
                throw new DirectoryNotFoundException(path);

            var assemblies = new List<Assembly>();

            foreach (var folder in Directory.EnumerateDirectories(path))
            {
                var foundValidPluginDLL = false;

                foreach (string dllPath in Directory.EnumerateFiles(folder, "*.Plugin.dll"))
                {
                    //AssemblyName assemblyName = AssemblyLoadContext.GetAssemblyName(dllPath);

                    Assembly assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(dllPath);

                    //Assembly assembly = AssemblyLoadContext.Default.LoadFromAssemblyName(assemblyName);

                    foreach (Type type in assembly.GetTypes())
                        if (typeof(IPlugin).GetTypeInfo().IsAssignableFrom(type) 
                            && typeof(BasePlugin).GetTypeInfo().IsAssignableFrom(type)
                            && type.GetTypeInfo().IsClass)
                        {
                            foundValidPluginDLL = true;
                            break;
                        }
                }

                if (!foundValidPluginDLL)
                    break;

                var dllsToAdd = Directory.EnumerateFiles(folder, "*.dll");
                foreach (string dllPath in dllsToAdd)
                {
                    //AssemblyName assemblyName = AssemblyLoadContext.GetAssemblyName(dllPath);
                   // Assembly assembly = AssemblyLoadContext.Default.LoadFromAssemblyName(assemblyName);

                    Assembly assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(dllPath);

                    if (IsCandidateAssembly(assembly))
                        assemblies.Add(assembly);
                }
            }


            foreach (CompilationLibrary compilationLibrary in DependencyContext.Default.CompileLibraries)
                if (IsCandidateCompilationLibrary(compilationLibrary))
                    assemblies.Add(Assembly.Load(new AssemblyName(compilationLibrary.Name)));

            return assemblies;
        }

        private static bool IsCandidateAssembly(Assembly assembly)
        {
            if (assembly.FullName.ToLower().Contains("modcore"))
                return false;

            return true;
        }

        private static bool IsCandidateCompilationLibrary(CompilationLibrary compilationLibrary)
        {
            if (compilationLibrary.Name.ToLower().StartsWith("modcore"))
                return false;

            if (!compilationLibrary.Dependencies.Any(d => d.Name.ToLower().Contains("modcore")))
                return false;

            return true;
        }
    }
}
