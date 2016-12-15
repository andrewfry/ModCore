using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using ModCore.Abstraction.Services.Access;
using ModCore.Models.Access;
using RoleBasedPermisison.Plugin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoleBasedPermisison.Abstraction
{
    public interface IPermissionService : IBasePermissionService
    {
        PermissionResult CheckPermission(Controller controller, ActionDescriptor action, User user);

        PermissionResult CheckPermission(Controller controller, ActionDescriptor action, List<string> userRoleIds);
    }
}
