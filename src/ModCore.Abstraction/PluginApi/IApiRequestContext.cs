using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModCore.Abstraction.PluginApi
{
    public interface IApiRequestContext
    {
         IServiceCollection Services { get; set; }
    }
}
