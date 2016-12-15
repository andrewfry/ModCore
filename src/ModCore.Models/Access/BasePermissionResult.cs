using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModCore.Models.Access
{
    public class BasePermissionResult
    {
        public PermissionExecutedResult PermissionExecutedResult { get; set; }

        public bool ErrorOccured { get; set; }

        public Exception Exception { get; set; }

      
    }

    public enum PermissionExecutedResult

    {
        Granted = 1,
        Denied = 2,
        NotDefined = 3
    }

}
