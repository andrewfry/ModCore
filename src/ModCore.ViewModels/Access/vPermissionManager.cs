using System;
using System.Collections.Generic;
using ModCore.Models.Access;
using System.Text;

namespace ModCore.ViewModels.Access
{
    public class vPermissionManager
    {
        public List<vRole> Roles { get; set; }

        public string SelectedRoleId { get; set; }

        public List<vPermissionDiscriptor> AvailablePermissions { get; set; }

        public vPermissionManager()
        {
            Roles = new List<vRole>();
            AvailablePermissions = new List<vPermissionDiscriptor>();
        }
    }
}
