using ModCore.Abstraction.DataAccess;
using ModCore.Models.Access;
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
    public class AllUserActivity : Specification<UserActivity>
    {

        public AllUserActivity()
        {
        }

        public override Expression<Func<UserActivity, bool>> IsSatisifiedBy()
        {
            return a => a != null;
        }
    }
}
