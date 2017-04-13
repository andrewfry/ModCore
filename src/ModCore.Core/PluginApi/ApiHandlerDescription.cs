using ModCore.Abstraction.PluginApi;
using ModCore.Abstraction.Plugins;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ModCore.Core.PluginApi
{

    public class ApiHandlerDescription : IApiHandlerDescription
    {
        public string ApiRequestName { get; set; }
        public IPlugin Plugin { get; set; }
        public Func<IApiArgument, IApiRequestContext, Task<IApiHandlerResponse>> Handler { get; set; }
    }


}
