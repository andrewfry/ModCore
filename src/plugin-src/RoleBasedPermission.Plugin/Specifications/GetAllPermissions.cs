using ModCore.Abstraction.DataAccess;
using ModCore.Models.Access;
using ModCore.Models.BaseEntities;
using ModCore.Specifications.BuiltIns;
using RoleBasedPermisison.Plugin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace RoleBasedPermission.Specifications
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
