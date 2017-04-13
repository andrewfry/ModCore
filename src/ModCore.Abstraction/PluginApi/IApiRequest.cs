using ModCore.Abstraction.PluginApi;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModCore.Abstraction.PluginApi
{
    public interface IApiRequest
    {
         string EventName { get; set; }
         IApiArgument Argument { get; set; }
         Action<IApiResponse> OnSuccess { get; set; }
         Action<IApiResponse> OnFailure { get; set; }
         ApiExecutionType Type { get; set; }
    }

    public enum ApiExecutionType
    {
        Single, // will fail if there are more than one event handlers
        First, // will use the first handler
        All //will use and return results from all of the handlers
    }
}
