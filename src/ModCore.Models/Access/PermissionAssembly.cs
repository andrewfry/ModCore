using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModCore.Models.Access
{
    public class PermissionAssembly : Permission
    {
        public string AssemblyName { get; set; }

        public List<PermissionArea> AreaPermissions { get; set; }
    }
}
