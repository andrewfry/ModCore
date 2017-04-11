using System;
using System.Collections.Generic;
using System.Text;

namespace ModCore.Models.PluginApi
{
    public class ApiHandlerResponse
    {
        public bool Success { get; set; }
        public object Value { get; set; }
    }
}
