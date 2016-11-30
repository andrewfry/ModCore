using ModCore.Abstraction.DataAccess;
using ModCore.Models.Plugins;
using ModCore.Specifications.BuiltIns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ModCore.Specifications.Plugins
{
    public class ByAssemblyName : Specification<InstalledPlugin>
    {
        public string _assemblyName;


        public ByAssemblyName(string assemblyName)
        {
            _assemblyName = assemblyName;
        }

        public override Expression<Func<InstalledPlugin, bool>> IsSatisifiedBy()
        {
            return x => x.PluginAssemblyName.ToLower() == _assemblyName.ToLower();
        }
    }
}
