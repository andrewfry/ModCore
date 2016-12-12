using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using ModCore.Models.Access;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModCore.Abstraction.Services.Access
{
    public interface IPermissionService
    {
        PermissionResult CheckPermission(Controller controller, ActionDescriptor action, User user);

        PermissionResult CheckPermission(Controller controller, ActionDescriptor action, List<string> userRoleIds);
    }
}
