using ModCore.Abstraction.Plugins.Builtins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ModCore.Core.Plugins.Descriptions
{
    public class OptionalPlugin : IPluginDependency
    {
        private readonly IPluginDescription _description;

        public bool Required { get { return false; } }

        public IPluginDescription Description { get { return _description; } }


        public OptionalPlugin(IPluginDescription description)
        {
            _description = description;
        }
    }

}
