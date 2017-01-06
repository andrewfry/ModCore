using ModCore.Abstraction.DataAccess;
using ModCore.Models.Access;
using ModCore.Models.BaseEntities;
using ModCore.Specifications.BuiltIns;
using RoleBasedPermission.Plugin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace RoleBasedPermission.Plugin.Specifications
{
    public class GetAllPermissions : Specification<PermissionAssembly>
    {

        public GetAllPermissions()
        {
        }

        public override Expression<Func<PermissionAssembly, bool>> IsSatisifiedBy()
        {
            return a => a != null ;
        }
    }
}
