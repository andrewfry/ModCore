using ModCore.Abstraction.Plugins;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Threading;

namespace ModCore.Core.Events
{
    public class EventManager
    {
        private static ConcurrentBag<EventDescription> EventHandlers = new ConcurrentBag<EventDescription>();
        private static ConcurrentQueue<EventRequest> EventRequests = new ConcurrentQueue<EventRequest>();

        public EventManager()
        {
            var timerCallback = new TimerCallback(async (e) => await FulFillQueue());
            var timer = new Timer(timerCallback, null, 0, 1000);
        }

        public void QueueEvent(string eventName, EventArgument argument, Action<EventResponse> onSuccess, Action<EventResponse> onFailure, EventExecutionType executionType = EventExecutionType.All)
        {
            var addToQueue = new EventRequest
            {
                EventName = eventName.ToUpper(),
                Argument = argument,
                OnSuccess = onSuccess,
                OnFailure = onFailure,
                 Type = executionType
            };

            EventRequests.Enqueue(addToQueue);
        }

        public async Task FullfilEvent(string eventName, EventArgument argument, Action<EventResponse> onSuccess, Action<EventResponse> onFailure, EventExecutionType executionType = EventExecutionType.All)
        {
            var addToQueue = new EventRequest
            {
                EventName = eventName.ToUpper(),
                Argument = argument,
                OnSuccess = onSuccess,
                OnFailure = onFailure,
                Type = executionType
            };

            await HandleEventRequest(addToQueue);
        }

        private static async Task FulFillQueue()
        {
            do
            {
                EventRequest req = null;

                var val = EventRequests.TryDequeue(out req);

                if (req != null)
                {
                    await HandleEventRequest(req);
                }
            }
            while (EventRequests.Count > 0);
        }

        private static async Task HandleEventRequest(EventRequest request)
        {
            try
            {
                var handlers = EventHandlers.Where(a => a.EventName == request.EventName.ToUpper()).ToList();
                if (!handlers.Any())
                    throw new Exception($"No handlers configured for {request.EventName}");

                object returnObj = null;

                if (handlers.Count == 1 || request.Type == EventExecutionType.First 
                    || request.Type == EventExecutionType.Single)
                {
                    if(request.Type == EventExecutionType.Single && handlers.Count > 1)
                        throw new Exception("Failed to process because EventExecutionType was Single and multiple event handlers were found");

                    var h = handlers.First();
                    var r = await h.Handler(request.Argument);
                    if (!r.Success)
                        throw new Exception("Failed to process correctly");
                    returnObj = r.Value;
                }
                else
                {
                    var values = new List<object>();
                    foreach (var h in handlers)
                    {
                        var r = await h.Handler(request.Argument);
                        if (!r.Success)
                            throw new Exception("Failed to process correctly");
                        values.Add(r.Value);
                    }
                    returnObj = values;
                }

                var response = new EventResponse()
                {
                    Value = returnObj,
                    Success = true,
                };

                request.OnFailure(response);
            }
            catch (Exception ex)
            {
                var response = new EventResponse()
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
