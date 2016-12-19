using System;
using System.Collections.Generic;
using ModCore.Models.Access;
using System.Text;
using ModCore.ViewModels.Access;
using ModCore.ViewModels.Base;

namespace RoleBasedPermission.Plugin.ViewModels
{
    public class vPermissionManager : BaseViewModel
    {
        public List<vRole> Roles { get; set; }

        public string SelectedRoleId { get; set; }


        public vPermissionManager()
        {
            Roles = new List<vRole>();
        }
    }
}
