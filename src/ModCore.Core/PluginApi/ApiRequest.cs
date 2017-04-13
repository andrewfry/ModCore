using ModCore.Abstraction.PluginApi;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModCore.Core.PluginApi
{
    public class ApiRequest : IApiRequest
    {
        public string EventName { get; set; }
        public IApiArgument Argument { get; set; }
        public Action<IApiResponse> OnSuccess { get; set; }
        public Action<IApiResponse> OnFailure { get; set; }
        public ApiExecutionType Type { get; set; }
    }

}
