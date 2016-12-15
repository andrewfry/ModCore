using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ModCore.Models.Enum;

namespace ModCore.ViewModels.Access
{
    public class vPermissionDiscriptor : vPermission
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string AssemblyName { get; set; }

        public bool IsPlugin { get; set; }

        public List<vPermissionAreaDiscriptor> AreaPermissons { get; set; }

        public vPermissionDiscriptor()
        {
            AreaPermissons = new List<vPermissionAreaDiscriptor>();
        }
    }

    public class vPermissionAreaDiscriptor : vPermission
    {
        public string Name { get; set; }

        public List<vPermissionControllerDiscriptor> ControllerPermissons { get; set; }

        public vPermissionAreaDiscriptor()
        {
            ControllerPermissons = new List<vPermissionControllerDiscriptor>();
        }

    }

    public class vPermissionControllerDiscriptor : vPermission
    {
        public string Name { get; set; }

        public List<vPermissionActionDiscriptor> ActionPermissons { get; set; }

        public vPermissionControllerDiscriptor()
        {
            ActionPermissons = new List<vPermissionActionDiscriptor>();
        }
    }

    public class vPermissionActionDiscriptor : vPermission
    {
        public string Name { get; set; }

        public string Route { get; set; }

        public HttpMethod? Method { get; set; }

    }

   public class vPermission
    {
        public string RoleId { get; set; }

        public bool AccessGranted { get; set; }
    }

}
