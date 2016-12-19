using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoleBasedPermission.Plugin.Models
{
    public class PermissionArea : Permission
    {
        public string AreaName { get; set; }

        public List<PermissionController> ControllerPermissons { get; set; } 
    }
}
