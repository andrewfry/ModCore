using ModCore.Abstraction.Plugins;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ModCore.Abstraction.PluginApi
{
    public interface IApiHandlerDescription
    {
         string ApiRequestName { get; set; }
         IPlugin Plugin { get; set; }
         Func<IApiArgument, IApiRequestContext, Task<IApiHandlerResponse>> Handler { get; set; }
    }
}
