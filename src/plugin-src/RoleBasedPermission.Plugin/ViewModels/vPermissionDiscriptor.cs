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

        public override bool AllChildrenEnabled { get { return (AreaPermissons.Count(a => a.AccessGranted) == AreaPermissons.Count()) &&
                   (AreaPermissons.Count(a => a.AllChildrenEnabled) == AreaPermissons.Count()); } }

        public override bool AnyChildrenEnabled { get { return AreaPermissons.Any(a => a.AnyChildrenEnabled); } }

        public vPermissionDiscriptor()
        {
            AreaPermissons = new List<vPermissionAreaDiscriptor>();
        }
    }

    public class vPermissionAreaDiscriptor : vPermission
    {
        public List<vPermissionControllerDiscriptor> ControllerPermissons { get; set; }

        public override bool HasChildren { get { return ControllerPermissons.Any(); } }

        public override bool AllChildrenEnabled { get { return ControllerPermissons.Count(a => a.AccessGranted) == ControllerPermissons.Count() &&
                    ControllerPermissons.Count(a => a.AllChildrenEnabled) == ControllerPermissons.Count(); } }

        public override bool AnyChildrenEnabled { get { return ControllerPermissons.Any(a => a.AnyChildrenEnabled); } }

        public vPermissionAreaDiscriptor()
        {
            ControllerPermissons = new List<vPermissionControllerDiscriptor>();
        }

    }

    public class vPermissionControllerDiscriptor : vPermission
    {
        public List<vPermissionActionDiscriptor> ActionPermissons { get; set; }

        public override bool HasChildren { get { return ActionPermissons.Any(); } }

        public override bool AllChildrenEnabled { get { return ActionPermissons.Count(a=>a.AccessGranted) == ActionPermissons.Count() &&
                   ActionPermissons.Count(a => a.AllChildrenEnabled) == ActionPermissons.Count(); } }

        public override bool AnyChildrenEnabled { get { return ActionPermissons.Any(a => a.AnyChildrenEnabled); } }


        public vPermissionControllerDiscriptor()
        {
            ActionPermissons = new List<vPermissionActionDiscriptor>();
        }
    }

    public class vPermissionActionDiscriptor : vPermission
    {
        public string Route { get; set; }

        public HttpMethod? Method { get; set; }

        public override bool AllChildrenEnabled { get { return this.AccessGranted; } }

        public override bool AnyChildrenEnabled { get { return this.AccessGranted; } }

        public override bool HasChildren { get { return false; } }

    }

    public class vPermission
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string RoleId { get; set; }

        public bool AccessGranted { get; set; }

        public virtual bool AllChildrenEnabled { get; }

        public virtual bool AnyChildrenEnabled { get; }

        public virtual bool HasChildren { get; }
    }

}
