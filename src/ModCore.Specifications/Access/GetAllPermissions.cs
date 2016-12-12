using ModCore.Abstraction.DataAccess;
using ModCore.Models.Access;
using ModCore.Models.BaseEntities;
using ModCore.Specifications.BuiltIns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ModCore.Specifications.Access
{
    public class GetAllPermissions : Specification<Permission>
    {

        public GetAllPermissions()
        {
        }

        public override Expression<Func<Permission, bool>> IsSatisifiedBy()
        {
            return a => a != null ;
        }
    }
}
