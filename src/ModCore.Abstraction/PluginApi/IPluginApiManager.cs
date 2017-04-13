using System;
using System.Collections.Generic;
using System.Text;
using ModCore.Abstraction.Plugins;
using System.Threading.Tasks;

namespace ModCore.Abstraction.PluginApi
{
    public interface IPluginApiManager
    {
        void RegisterApiRequestHander(string apiRequestName, IPlugin plugin, Func<IApiArgument, IApiRequestContext, Task<IApiHandlerResponse>> handler);

        void QueueApiRequest(string apiRequestName, IApiArgument argument, Action<IApiResponse> onSuccess, Action<IApiResponse> onFailure, ApiExecutionType executionType = ApiExecutionType.All);

        Task<IApiResponse> FullfilApiRequest(string apiRequestName, IApiArgument argument, IApiRequestContext context, ApiExecutionType executionType = ApiExecutionType.All);
    }
}
