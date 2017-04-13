using Microsoft.Extensions.DependencyInjection;
using ModCore.Abstraction.PluginApi;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModCore.Core.PluginApi
{
    public class ApiRequestContext : IApiRequestContext
    {
        public IServiceCollection Services { get; set; }
    }
}
