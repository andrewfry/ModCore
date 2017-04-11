using ModCore.Abstraction.Plugins;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ModCore.Models.PluginApi
{

    public class ApiHandlerDescription
    {
        public string ApiRequestName { get; set; }
        public IPlugin Plugin { get; set; }
        public Func<ApiArgument, Task<ApiHandlerResponse>> Handler { get; set; }
    }

   
}
