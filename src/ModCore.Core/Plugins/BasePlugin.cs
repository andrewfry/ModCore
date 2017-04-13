using ModCore.Models.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text;

namespace ModCore.Core.Plugins
{
    public abstract class BasePlugin
    {

        public abstract string Name { get; }

        public abstract string Version { get; }

        private string _pluginHash;

        public BasePlugin()
        {
            _pluginHash = null;
        }

        public string AssemblyName
        {
            get
            {
                return this.GetType().Namespace;
            }
        }

        public string PluginHash
        {
            get
            {
                if (_pluginHash == null)
                {
                    var key = string.Concat(this.AssemblyName, this.Name, this.Version);
                    var sha1 = SHA1.Create();
                    var newHashBytes = Encoding.ASCII.GetBytes(key);
                    _pluginHash = Encoding.ASCII.GetString(sha1.ComputeHash(newHashBytes));
                }
                return _pluginHash;
            }
        }

        public virtual PluginResult Install(PluginInstallContext context)
        {
            var result = new PluginResult();
            result.WasSuccessful = true;
            return result;
        }

        public virtual PluginResult StartUp(PluginStartupContext context)
        {
            var result = new PluginResult();
            result.WasSuccessful = true;
            return result;
        }

        public virtual PluginResult UnInstall(PluginUninstallContext context)
        {
            var result = new PluginResult();
            result.WasSuccessful = true;
            return result;
        }

    }
}
