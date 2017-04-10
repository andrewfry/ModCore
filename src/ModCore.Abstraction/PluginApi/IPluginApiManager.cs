using System;
using System.Collections.Generic;
using System.Text;
using ModCore.Models.PluginApi;
using ModCore.Abstraction.Plugins;
using System.Threading.Tasks;

namespace ModCore.Abstraction.PluginApi
{
    public interface IPluginApiManager
    {
        void RegisterApiHander(string apiRequestName, IPlugin plugin, Func<ApiArgument, Task<ApiHandlerResponse>> handler);

        void QueueApiRequest(string apiRequestName, ApiArgument argument, Action<ApiResponse> onSuccess, Action<ApiResponse> onFailure, ApiExecutionType executionType = ApiExecutionType.All);

        Task<ApiResponse> FullfilEventApiRequest(string apiRequestName, ApiArgument argument, ApiExecutionType executionType = ApiExecutionType.All);
    }
}
