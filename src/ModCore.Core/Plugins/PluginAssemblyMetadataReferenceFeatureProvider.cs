using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Razor.Compilation;
using Microsoft.CodeAnalysis;
using ModCore.Abstraction.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModCore.Core.Plugins
{
    public class PluginAssemblyMetadataReferenceFeatureProvider : IApplicationFeatureProvider<MetadataReferenceFeature>
    {

        private readonly IPluginManager _pluginManager;

        public PluginAssemblyMetadataReferenceFeatureProvider(IPluginManager pluginManager)
        {
            _pluginManager = pluginManager;
        }

        public void PopulateFeature(IEnumerable<ApplicationPart> parts, MetadataReferenceFeature feature)
        {


            foreach (var assemblyTuple in _pluginManager.ActivePluginAssemblies)
            {
                //var pluginAssembly = assemblyTuple.Item2.SingleOrDefault(a => a.GetType().Namespace == assemblyTuple.Item1.AssemblyName);
                //var dependencies = assemblyTuple.Item2.Where(a => a.GetType().Namespace != assemblyTuple.Item1.AssemblyName).ToList();
                //var assemblyPart = new AssemblyPart(pluginAssembly);
                //var metaDataRefFeature = new MetadataReferenceFeature();

                foreach (var assembly in assemblyTuple.Item2)
                {
                    var metaReference = MetadataReference.CreateFromFile(assembly.Location);
                    feature.MetadataReferences.Add(metaReference);
                }
            }

        }
    }
}
