using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ModCore.Models.Enum;
using ModCore.ViewModels.Base;

namespace RoleBasedPermission.Plugin.ViewModels
{
    public class vPermissionDiscriptorEdit : BaseViewModel
    {
        public List<vPermissionDiscriptor> Permissons { get; set; }

        public vPermissionDiscriptorEdit()
        {
            Permissons = new List<vPermissionDiscriptor>();
        }
    }



}
