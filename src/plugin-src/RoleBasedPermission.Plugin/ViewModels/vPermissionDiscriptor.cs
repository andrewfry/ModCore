using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ModCore.Models.Enum;

namespace RoleBasedPermission.Plugin.ViewModels
{
    public class vPermissionDiscriptor : vPermission
    {

        public string Description { get; set; }

        public string AssemblyName { get; set; }

        public bool IsPlugin { get; set; }

        public override bool HasChildren { get { return AreaPermissons.Any(); } }

        public List<vPermissionAreaDiscriptor> AreaPermissons { get; set; }

        public vPermissionDiscriptor()
        {
            AreaPermissons = new List<vPermissionAreaDiscriptor>();
        }
    }

    public class vPermissionAreaDiscriptor : vPermission
    {
        public List<vPermissionControllerDiscriptor> ControllerPermissons { get; set; }

        public override bool HasChildren { get { return ControllerPermissons.Any(); } }

        public vPermissionAreaDiscriptor()
        {
            ControllerPermissons = new List<vPermissionControllerDiscriptor>();
        }

    }

    public class vPermissionControllerDiscriptor : vPermission
    {
        public List<vPermissionActionDiscriptor> ActionPermissons { get; set; }

        public override bool HasChildren { get { return ActionPermissons.Any(); } }

        public vPermissionControllerDiscriptor()
        {
            ActionPermissons = new List<vPermissionActionDiscriptor>();
        }
    }

    public class vPermissionActionDiscriptor : vPermission
    {
        public string Route { get; set; }

        public HttpMethod? Method { get; set; }

        public override bool HasChildren { get { return false; } }

    }

    public class vPermission
    {
        public string Name { get; set; }

        public string RoleId { get; set; }

        public bool AccessGranted { get; set; }

        public virtual bool HasChildren { get; }
    }

}
