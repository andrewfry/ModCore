using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using ModCore.Abstraction.Services.Access;
using ModCore.Models.Access;
using RoleBasedPermission.Plugin.Models;
using System.Collections.Generic;

namespace RoleBasedPermission.Plugin.Abstraction
{
    public interface IPermissionService : IBasePermissionService
    {
        PermissionResult CheckPermission(Controller controller, ActionDescriptor action, User user);

        PermissionResult CheckPermission(Controller controller, ActionDescriptor action, List<string> userRoleIds);
    }
}
