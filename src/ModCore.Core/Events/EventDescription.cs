using ModCore.Abstraction.Plugins;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ModCore.Core.Events
{

    public class EventDescription
    {
        public string EventName { get; set; }
        public object Argument { get; set; }
        public IPlugin Plugin { get; set; }
        public Func<EventArgument, Task<EventHandlerResponse>> Handler { get; set; }
    }

   
}
