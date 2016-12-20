using ModCore.Models.Access;
using ModCore.Models.BaseEntities;
using System.Collections.Generic;

namespace RoleBasedPermission.Plugin.Models
{
    public class Permission : BaseEntity
    {
        public List<string> GrantedRoles { get; set; }

        public List<string> DeniedRoles { get; set; }
    }
}
