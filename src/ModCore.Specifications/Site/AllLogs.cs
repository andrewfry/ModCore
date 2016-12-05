using ModCore.Abstraction.DataAccess;
using ModCore.Models.BaseEntities;
using ModCore.Models.Core;
using ModCore.Specifications.BuiltIns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ModCore.Specifications.Site
{
    public class AllLogs : Specification<Log>
    {

        public AllLogs()
        {
        }

        public override Expression<Func<Log, bool>> IsSatisifiedBy()
        {
            return a => a != null;
        }
    }
}
