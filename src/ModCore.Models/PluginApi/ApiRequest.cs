using System;
using System.Collections.Generic;
using System.Text;

namespace ModCore.Models.PluginApi
{
    public class ApiRequest
    {
        public string EventName { get; set; }
        public ApiArgument Argument { get; set; }
        public Action<ApiResponse> OnSuccess { get; set; }
        public Action<ApiResponse> OnFailure { get; set; }
        public ApiExecutionType Type { get; set; }
    }

    public enum ApiExecutionType
    {
        Single, // will fail if there are more than one event handlers
        First, // will use the first handler
        All //will use and return results from all of the handlers
    }
}
