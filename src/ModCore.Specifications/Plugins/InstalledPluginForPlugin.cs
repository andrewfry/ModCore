using ModCore.Abstraction.DataAccess;
using ModCore.Abstraction.Plugins;
using ModCore.Models.Plugins;
using ModCore.Specifications.BuiltIns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ModCore.Specifications.Plugins
{
    public class InstalledPluginForPlugin : Specification<InstalledPlugin>
    {
        private IPlugin _plugin;

        public InstalledPluginForPlugin(IPlugin plugin)
        {
            _plugin = plugin;
        }

        public override Expression<Func<InstalledPlugin, bool>> IsSatisifiedBy()
        {
            return x => x.PluginAssemblyName == _plugin.AssemblyName;
        }
    }
}
