using ModCore.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModCore.Models.Access
{
    public class PermissionAction : Permission
    {
        public string ActionName { get; set; }

        public HttpMethod Method { get; set; }
    }
}
