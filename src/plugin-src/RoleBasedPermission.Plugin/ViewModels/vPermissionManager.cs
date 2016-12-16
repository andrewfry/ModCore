using System;
using System.Collections.Generic;
using ModCore.Models.Access;
using System.Text;
using ModCore.ViewModels.Access;

namespace RoleBasedPermisison.ViewModels
{
    public class vPermissionManager
    {
        public List<vRole> Roles { get; set; }

        public string SelectedRoleId { get; set; }


        public vPermissionManager()
        {
            Roles = new List<vRole>();
        }
    }
}
