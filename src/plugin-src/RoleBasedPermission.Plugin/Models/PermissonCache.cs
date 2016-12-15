using ModCore.Models.Access;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoleBasedPermisison.Plugin.Models
{
    public class PermissonCache
    {
        private static Hashtable _internalCache;
        private static int _version;

        public int Version { get; set; }

        private Hashtable Cache
        {
            get
            {
                if (_internalCache == null || _version != Version)
                {
                    _internalCache = new Hashtable();
                    _version = Version;
                }

                return _internalCache;
            }
        }

        public PermissonCache()
        {
            _version = 0;
            Version = 0;
        }

        public void AddToCache(string assemblyName, string areaName, string controllerName, string actionName, string roleId, PermissionExecutedResult permissionResult)
        {
            var key = GetKey(assemblyName, areaName, controllerName, actionName, roleId);
            Cache.Add(key, permissionResult);
        }

        public PermissionExecutedResult? FromCache(string assemblyName, string areaName, string controllerName, string actionName, string roleId)
        {
            var key = GetKey(assemblyName, areaName, controllerName, actionName, roleId);
            return Cache.ContainsKey(key) ? ((PermissionExecutedResult?)Cache[key]) : null;
        }

        private string GetKey(string assemblyName, string areaName, string controllerName, string actionName, string roleId)
        {
            return string.Concat(assemblyName.ToUpper(), areaName.ToUpper(), controllerName.ToUpper(), actionName.ToUpper(), roleId.ToUpper());
        }
    }
}
