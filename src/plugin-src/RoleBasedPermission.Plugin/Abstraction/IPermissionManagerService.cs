using RoleBasedPermisison.Plugin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoleBasedPermisison.Abstraction
{
    public interface IPermissionManagerService
    {
        List<PermissionDiscriptor> GetControllerDiscriptor();
    }
}
