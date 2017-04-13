using ModCore.Abstraction.PluginApi;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModCore.Core.PluginApi
{
    public class ApiHandlerResponse : IApiHandlerResponse
    {
        public bool Success { get; set; }
        public object Value { get; set; }
    }
}
