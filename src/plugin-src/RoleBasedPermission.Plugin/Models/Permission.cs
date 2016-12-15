using ModCore.Models.Access;
using ModCore.Models.BaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoleBasedPermisison.Plugin.Models
{
    public class Permission : BaseEntity
    {
        public List<Role> GrantedRoles { get; set; }

        public List<Role> DeniedRoles { get; set; }
    }
}
