using ModCore.Abstraction.Site;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModCore.Abstraction.Plugins
{
    public interface IPluginLog : ILog
    {
        void SetPlugin(IPlugin plugin);
    }
}
