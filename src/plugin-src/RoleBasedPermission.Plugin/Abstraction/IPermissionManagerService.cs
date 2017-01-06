using RoleBasedPermission.Plugin.Models;
using RoleBasedPermission.Plugin.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RoleBasedPermission.Plugin.Abstraction
{
    public interface IPermissionManagerService
    {
        List<PermissionDiscriptor> GetControllerDiscriptor();

        List<vPermissionDiscriptor> GetDiscriptorsForRole(string roleId);

        Task UpdateDiscriptorsAsync(List<vPermissionDiscriptor> discriptors, string roleId);
    }
}