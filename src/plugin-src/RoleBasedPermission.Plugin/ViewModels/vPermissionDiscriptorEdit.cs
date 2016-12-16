using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ModCore.Models.Enum;

namespace RoleBasedPermisison.ViewModels
{
    public class vPermissionDiscriptorEdit 
    {
        public List<vPermissionDiscriptor> Permissons { get; set; }

        public vPermissionDiscriptorEdit()
        {
            Permissons = new List<vPermissionDiscriptor>();
        }
    }



}
