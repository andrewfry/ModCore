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
    public class GetPermissionByRole : Specification<Permission>
    {

        private string _roleId;

        public GetPermissionByRole(string roleId)
        {
            this._roleId = roleId;
        }

        public override Expression<Func<Permission, bool>> IsSatisifiedBy()
        {
            return a => a.GrantedRoles.Any(r => r == _roleId);
        }
    }
}
