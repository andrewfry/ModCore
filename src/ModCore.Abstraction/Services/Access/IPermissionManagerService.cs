using ModCore.Models.Access;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModCore.Abstraction.Services.Access
{
    public interface IPermissionManagerService
    {
        List<PermissionDiscriptor> GetControllerDiscriptor();
    }
}
