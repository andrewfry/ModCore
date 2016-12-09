using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModCore.Models.Access
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

        public void AddToCache(string assemblyName, string areaName, string controllerName, string actionName, string roleId, bool accessAllowed)
        {
            var key = GetKey(assemblyName, areaName, controllerName, actionName, roleId);
            Cache.Add(key, accessAllowed);
        }

        public bool? FromCache(string assemblyName, string areaName, string controllerName, string actionName, string roleId, bool accessAllowed)
        {
            var key = GetKey(assemblyName, areaName, controllerName, actionName, roleId);

            return (bool?)Cache[key];
        }

        private string GetKey(string assemblyName, string areaName, string controllerName, string actionName, string roleId)
        {
            return string.Concat(assemblyName.ToUpper(), areaName.ToUpper(), controllerName.ToUpper(), actionName.ToUpper(), roleId.ToUpper());
        }
    }
}
