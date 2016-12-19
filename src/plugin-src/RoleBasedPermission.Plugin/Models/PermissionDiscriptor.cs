using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ModCore.Models.Enum;

namespace RoleBasedPermission.Plugin.Models
{
    public class PermissionDiscriptor
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string AssemblyName { get; set; }

        public bool IsPlugin { get; set; }

        public List<PermissionAreaDiscriptor> AreaPermissons { get; set; }

        public PermissionDiscriptor()
        {
            AreaPermissons = new List<PermissionAreaDiscriptor>();
        }
    }

    public class PermissionAreaDiscriptor
    {
        public string Name { get; set; }

        public List<PermissionControllerDiscriptor> ControllerPermissons { get; set; }

        public PermissionAreaDiscriptor()
        {
            ControllerPermissons = new List<PermissionControllerDiscriptor>();
        }

    }

    public class PermissionControllerDiscriptor
    {
        public string Name { get; set; }

        public List<PermissionActionDiscriptor> ActionPermissons { get; set; }

        public PermissionControllerDiscriptor()
        {
            ActionPermissons = new List<PermissionActionDiscriptor>();
        }
    }

    public class PermissionActionDiscriptor
    {
        public string Name { get; set; }

        public string Route { get; set; }

        public HttpMethod? Method { get; set; }

    }

   

}
