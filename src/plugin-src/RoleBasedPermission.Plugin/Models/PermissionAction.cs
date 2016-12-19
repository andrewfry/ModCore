using ModCore.Models.BaseEntities;
using ModCore.Models.Enum;
using System.Collections.Generic;

namespace RoleBasedPermission.Plugin.Models
{
    public class PermissionAction : Permission
    {
        public string ActionName { get; set; }

        public HttpMethod Method { get; set; }
    }
}
