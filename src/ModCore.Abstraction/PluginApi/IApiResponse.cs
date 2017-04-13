using System;
using System.Collections.Generic;
using System.Text;

namespace ModCore.Abstraction.PluginApi
{
    public interface IApiResponse
    {
        object Value { get; set; }
        bool Success { get; set; }
        List<string> HandledBy { get; set; }
        Exception Exception { get; set; }

        T GetValue<T>();
    }
}
