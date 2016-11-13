using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModCore.ViewModels.Admin.Plugin
{
    public class vPlugin
    {

        public string Name { get; set; }

        public string Version { get; set; }

        public string AssemblyName { get; set; }

        public string Description { get; set; }

        public bool Installed { get; set; }

        public bool Active { get; set; }

        public string PluginAssemblyName { get; set; }

        public DateTime DateInstalled { get; set; }

        public DateTime DateActivated { get; set; }

        public DateTime DateDeactivated { get; set; }
    }
}
