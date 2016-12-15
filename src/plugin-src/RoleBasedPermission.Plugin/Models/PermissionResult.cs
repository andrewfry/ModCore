using ModCore.Models.Access;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoleBasedPermisison.Plugin.Models 
{
    public class PermissionResult : BasePermissionResult
    {
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
            switch (enumVal)
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

   

}
