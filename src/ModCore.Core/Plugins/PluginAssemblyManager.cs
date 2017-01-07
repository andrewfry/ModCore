using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyModel;
using ModCore.Abstraction.Plugins;
using System.Runtime.Loader;
using ModCore.Core.HelperExtensions;
using Microsoft.Extensions.Logging;
using ModCore.Utilities.HelperExtensions;
using ModCore.Models.Plugins;

namespace ModCore.Core.Plugins
{
    public class PluginAssemblyManager : IAssemblyManager
    {

        private readonly ILogger _logger;

        public PluginAssemblyManager(ILogger logger)
        {
            _logger = logger;
        }

        internal static HashSet<string> ReferenceAssembliesStartsWith { get; } = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "ModCore"
        };

        private List<Assembly> _coreApplicationAssemblies;
        private List<Dependency> _dependencies;
        private List<PluginError> _errors;

        public IList<PluginError> PluginErrors
        {
            get
            {
                if (_errors == null)
                {
                    _errors = new List<PluginError>();
                }

                return _errors;
            }
        }

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

        public IEnumerable<Assembly> GetReferencingAssemblies(string assemblyNameStartsWith)
        {
            var assemblies = new List<Assembly>();
            var dependencies = DependencyContext.Default.RuntimeLibraries;
            foreach (var library in dependencies)
            {
                try
                {
                    if (IsCandidateLibrary(library, assemblyNameStartsWith))
                    {
                        var assembly = Assembly.Load(new AssemblyName(library.Name));
                        assemblies.Add(assembly);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(new EventId(), ex, "Failed to load the library {0}.", library?.Name);
                    PluginErrors.Add(new PluginError(PluginErrorLevel.Error, PluginErrorType.AssemblyLoad, library?.Name, "Failed to load a reference library."));
                }
            }
            return assemblies;
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
                    try
                    {
                        var assemblyName = FromPath(dllPath);
                        if (!AlreadyLoaded(assemblyName))
                        {
                            Assembly assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(dllPath);
                            assemblies.Add(assembly);
                        }
                        else
                        {
                            _logger.LogWarning(new EventId(), "This DLL is already loaded.", dllPath);
                            PluginErrors.Add(new PluginError(PluginErrorLevel.Warning, PluginErrorType.AssemblyLoad, dllPath, "This DLL is already loaded."));
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(new EventId(), ex, "The dll {0} did not load the DLL correctly.", dllPath);
                        PluginErrors.Add(new PluginError(PluginErrorLevel.Error, PluginErrorType.AssemblyLoad, dllPath, ex.Message));
                    }
                }
            }

            return assemblies;
        }

        public IList<Tuple<IPlugin, IList<Assembly>>> GetPluginAndAssemblies(string path)
        {
            var tuples = new List<Tuple<IPlugin, IList<Assembly>>>();

            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException("path");

            if (!Directory.Exists(path))
                throw new DirectoryNotFoundException(path);


            foreach (var folder in Directory.EnumerateDirectories(path))
            {
                var assemblies = new List<Assembly>();
                IPlugin pluginInstance = null;

                var dllsToAdd = Directory.EnumerateFiles(folder, "*.dll");
                if (!dllsToAdd.Any())
                {
                    _logger.LogWarning(new EventId(), "There are no plugins found in the Plugin location.", folder);
                    PluginErrors.Add(new PluginError(PluginErrorLevel.Warning, PluginErrorType.AssemblyLoad, folder, "There are no plugins found in the Plugin location."));
                }

                foreach (string dllPath in dllsToAdd)
                {
                    try
                    {
                        var assemblyName = FromPath(dllPath);

                        if (!AlreadyLoaded(assemblyName))
                        {
                            Assembly assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(dllPath);
                            assemblies.Add(assembly);

                            var pluginType = assembly.GetImplementationOrDefault<IPlugin>();
                            if (pluginType != null)
                            {
                                pluginInstance = assembly.GetInstance<IPlugin>();
                            }
                            else
                            {
                                _logger.LogWarning(new EventId(), "This DLL does not contain an IPlugin class.", dllPath);
                                PluginErrors.Add(new PluginError(PluginErrorLevel.Warning, PluginErrorType.AssemblyLoad, dllPath, "This DLL does not contain an IPlugin class."));
                            }
                        }
                        else
                        {
                            _logger.LogWarning(new EventId(), "This DLL is already loaded.", dllPath);
                            PluginErrors.Add(new PluginError(PluginErrorLevel.Warning, PluginErrorType.AssemblyLoad, dllPath, "This DLL is already loaded."));
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(new EventId(), ex, "The dll {0} did not load the DLL correctly.", dllPath);
                        PluginErrors.Add(new PluginError(PluginErrorLevel.Error, PluginErrorType.AssemblyLoad, dllPath, ex.Message));
                    }
                }

                if (pluginInstance != null)
                    tuples.Add(new Tuple<IPlugin, IList<Assembly>>(pluginInstance, assemblies));

            }

            return tuples;
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

        private AssemblyName FromPath(string dllPath)
        {
            var pieces = dllPath.Split('\\').ToArray();
            var dllName = pieces[pieces.Length - 1];
            var assemblyName = dllName.Substring(0, dllName.Length - 4);
            return new AssemblyName(assemblyName);
        }

        private IEnumerable<Dependency> GetDependencies(string assemblyStartsWith)
        {
            var dependencies = new HashSet<Dependency>();

            var depcontext = DependencyContext.Default.RuntimeLibraries;
            foreach (var library in depcontext)
            {
                if (IsCandidateLibrary(library, assemblyStartsWith))
                {
                    foreach (var dep in library.Dependencies)
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
                || ApplicationDependencies.Any(a => a.Name == assemblyName.Name);
        }

    }
}
