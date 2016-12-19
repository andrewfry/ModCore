using ModCore.Models.Access;
using ModCore.Models.BaseEntities;
using System.Collections.Generic;

namespace RoleBasedPermission.Plugin.Models
{
    public class Permission : BaseEntity
    {
        public List<Role> GrantedRoles { get; set; }

        public List<Role> DeniedRoles { get; set; }
    }
}
