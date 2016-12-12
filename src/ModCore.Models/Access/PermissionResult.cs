using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModCore.Models.Access
{
    public class PermissionResult
    {
        public PermissionExecutedResult PermissionExecutedResult { get; set; }

        public bool ErrorOccured { get; set; }

        public Exception Exception { get; set; }

        public static PermissionResult Granted()
        {
            return new PermissionResult
            {
                PermissionExecutedResult = PermissionExecutedResult.Granted
            };
        }

        public static PermissionResult Denied()
        {
            return new PermissionResult
            {
                PermissionExecutedResult = PermissionExecutedResult.Denied
            };
        }

        public static PermissionResult NotDefined()
        {
            return new PermissionResult
            {
                PermissionExecutedResult = PermissionExecutedResult.NotDefined
            };
        }

        public static PermissionResult Error(Exception ex)
        {
            return new PermissionResult
            {
                PermissionExecutedResult = PermissionExecutedResult.NotDefined,
                ErrorOccured = true,
                Exception = ex
            };
        }

        public static PermissionResult FromEnum(PermissionExecutedResult enumVal)
        {
            switch(enumVal)
            {
                case PermissionExecutedResult.Granted:
                    return Granted();
                case PermissionExecutedResult.Denied:
                    return Denied();
                case PermissionExecutedResult.NotDefined:
                    return NotDefined();
                default:
                    throw new NotSupportedException($"The enum value {enumVal.ToString()} is not supported.");   
            }
        }
    }

    public enum PermissionExecutedResult

    {
        Granted = 1,
        Denied = 2,
        NotDefined = 3
    }

}
