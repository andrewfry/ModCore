using ModCore.Abstraction.Plugins.Builtins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ModCore.Core.Plugins.Descriptions
{
    public class RequiredPlugin : IPluginDependency
    {
        private readonly IPluginDescription _description;

        public bool Required { get { return true; } }

        public IPluginDescription Description { get { return _description; } }


        public RequiredPlugin(IPluginDescription description)
        {
            _description = description;
        }
    }

}
