using RoleBasedPermission.Plugin.Models;
using RoleBasedPermission.Plugin.ViewModels;
using System.Collections.Generic;

namespace RoleBasedPermission.Plugin.Abstraction
{
    public interface IPermissionManagerService
    {
        List<PermissionDiscriptor> GetControllerDiscriptor();

        List<vPermissionDiscriptor> GetDiscriptorsForRole(string roleId);

        void UpdateDiscriptors(List<vPermissionDiscriptor> discriptors);
    }
}