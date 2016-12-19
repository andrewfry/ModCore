using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoleBasedPermission.Plugin.Models
{
    public class PermissionController : Permission
    {
        public string ControllerName { get; set; }

        public List<PermissionAction> ActionPermissons { get; set; }
    }
}
