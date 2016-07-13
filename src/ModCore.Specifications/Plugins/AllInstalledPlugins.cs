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
    public class AllActivePlugins : Specification<InstalledPlugin>
    {

        public AllActivePlugins()
        {
        }

        public override Expression<Func<InstalledPlugin, bool>> IsSatisifiedBy()
        {
            return x => x.Installed && x.Active;
        }
    }
}
