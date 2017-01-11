using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModCore.Models.Plugins
{
    public abstract class PluginContext
    {
        public IServiceProvider ServiceProvider { get; set; }

    }
}
