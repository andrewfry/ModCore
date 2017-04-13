using ModCore.Abstraction.Plugins;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Threading;
using ModCore.Abstraction.PluginApi;

namespace ModCore.Core.PluginApi
{
    public class PluginApiManager : IPluginApiManager
    {
        private static BlockingCollection<ApiHandlerDescription> ApiHandlers = new BlockingCollection<ApiHandlerDescription>();
        private static ConcurrentQueue<ApiRequest> ApiRequests = new ConcurrentQueue<ApiRequest>();
        private IApiRequestContext _defaultContext;

        public PluginApiManager(IApiRequestContext defaultContext)
        {
            _defaultContext = defaultContext;
            var timerCallback = new TimerCallback(async (e) => await FulFillQueue(_defaultContext));
            var timer = new Timer(timerCallback, null, 0, 1000);
        }

        public void RegisterApiRequestHander(string apiRequestName, IPlugin plugin, Func<IApiArgument, IApiRequestContext, Task<IApiHandlerResponse>> handler)
        {
            var registeredHandler = new ApiHandlerDescription
            {
                ApiRequestName = apiRequestName.ToUpper(),
                Plugin = plugin,
                Handler = handler
            };

            ApiHandlers.Add(registeredHandler);
        }

        public void QueueApiRequest(string apiRequestName, IApiArgument argument, Action<IApiResponse> onSuccess, Action<IApiResponse> onFailure, ApiExecutionType executionType = ApiExecutionType.All)
        {
            var addToQueue = new ApiRequest
            {
                EventName = apiRequestName.ToUpper(),
                Argument = argument,
                OnSuccess = onSuccess,
                OnFailure = onFailure,
                Type = executionType
            };

            ApiRequests.Enqueue(addToQueue);
        }

        public async Task<IApiResponse> FullfilApiRequest(string apiRequestName, IApiArgument argument, IApiRequestContext context, ApiExecutionType executionType = ApiExecutionType.All)
        {
            var addToQueue = new ApiRequest
            {
                EventName = apiRequestName.ToUpper(),
                Argument = argument,
                Type = executionType
            };

            return await HandleApiRequestWithResponse(addToQueue, context);
        }

        private static async Task FulFillQueue(IApiRequestContext context)
        {
            do
            {
                ApiRequest req = null;

                var val = ApiRequests.TryDequeue(out req);

                if (req != null)
                {
                    await HandleApiRequest(req, context);
                }
            }
            while (ApiRequests.Count > 0);
        }

        private static async Task<IApiResponse> HandleApiRequestWithResponse(IApiRequest request, IApiRequestContext context)
        {
            try
            {
                var handlers = ApiHandlers.Where(a => a.ApiRequestName == request.EventName.ToUpper()).ToList();
                if (!handlers.Any())
                    throw new Exception($"No handlers configured for {request.EventName}");

                object returnObj = null;
                List<string> handledBy = new List<string>();

                if (handlers.Count == 1 || request.Type == ApiExecutionType.First
                    || request.Type == ApiExecutionType.Single)
                {
                    if (request.Type == ApiExecutionType.Single && handlers.Count > 1)
                        throw new Exception("Failed to process because EventExecutionType was Single and multiple event handlers were found");

                    var h = handlers.First();
                    var r = await h.Handler(request.Argument, context);
                    if (!r.Success)
                        throw new Exception("Failed to process correctly");
                    returnObj = r.Value;

                    if (h.Plugin != null)
                        handledBy.Add(string.Concat(h.Plugin.Name, "_", h.Plugin.Version));
                    else
                        handledBy.Add("Internal");
                }
                else
                {
                    var values = new List<object>();
                    foreach (var h in handlers)
                    {
                        var r = await h.Handler(request.Argument, context);
                        if (!r.Success)
                            throw new Exception("Failed to process correctly");
                        values.Add(r.Value);

                        if (h.Plugin != null)
                            handledBy.Add(string.Concat(h.Plugin.Name, "_", h.Plugin.Version));
                        else
                            handledBy.Add("Internal");
                    }
                    returnObj = values;
                }

                return new ApiResponse()
                {
                    Value = returnObj,
                    Success = true,
                    HandledBy = handledBy
                };

            }
            catch (Exception ex)
            {
                return new ApiResponse()
                {
                    Value = null,
                    Success = false,
                    Exception = ex
                };

            }
        }

        private static async Task HandleApiRequest(IApiRequest request, IApiRequestContext context)
        {
            try
            {
                var handlers = ApiHandlers.Where(a => a.ApiRequestName == request.EventName.ToUpper()).ToList();
                if (!handlers.Any())
                    throw new Exception($"No handlers configured for {request.EventName}");

                object returnObj = null;

                if (handlers.Count == 1 || request.Type == ApiExecutionType.First
                    || request.Type == ApiExecutionType.Single)
                {
                    if (request.Type == ApiExecutionType.Single && handlers.Count > 1)
                        throw new Exception("Failed to process because EventExecutionType was Single and multiple event handlers were found");

                    var h = handlers.First();
                    var r = await h.Handler(request.Argument, context);
                    if (!r.Success)
                        throw new Exception("Failed to process correctly");
                    returnObj = r.Value;
                }
                else
                {
                    var values = new List<object>();
                    foreach (var h in handlers)
                    {
                        var r = await h.Handler(request.Argument, context);
                        if (!r.Success)
                            throw new Exception("Failed to process correctly");
                        values.Add(r.Value);
                    }
                    returnObj = values;
                }

                var response = new ApiResponse()
                {
                    Value = returnObj,
                    Success = true,
                };

                request.OnSuccess(response);
            }
            catch (Exception ex)
            {
                var response = new ApiResponse()
                {
                    Value = null,
                    Success = false,
                    Exception = ex
                };

                request.OnFailure(response);

            }

        }

    }


    //internal class EventCache
    //{
    //    private static List<EventDefinition> _internalCache;
    //    private static int _version;

    //    public int Version { get; set; }

    //    public EventCache()
    //    {
    //        _version = 0;
    //        Version = 0;
    //    }

    //    private List<EventDefinition> Cache
    //    {
    //        get
    //        {
    //            if (_internalCache == null || _version != Version)
    //            {
    //                _internalCache = new List<EventDefinition>();
    //                _version = Version;
    //            }

    //            return _internalCache;
    //        }
    //    }

    //}


}
