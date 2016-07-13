using ModCore.Models.BaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModCore.Models.Plugins
{
    public class InstalledPlugin : BaseEntity
    {
        public string PluginName { get; set; }

        public string PluginVersion { get; set; }

        public bool Installed { get; set; }

        public bool Active { get; set; }

        public string PluginAssemblyName { get; set; }

        public DateTime DateInstalled { get; set; }

    }
}
