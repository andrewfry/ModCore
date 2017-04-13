using System;
using System.Collections.Generic;
using System.Text;

namespace ModCore.Abstraction.PluginApi
{
    public interface IApiHandlerResponse
    {
         bool Success { get; set; }
         object Value { get; set; }
    }
}
