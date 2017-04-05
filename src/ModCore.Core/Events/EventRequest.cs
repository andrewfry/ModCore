using System;
using System.Collections.Generic;
using System.Text;

namespace ModCore.Core.Events
{
    public class EventRequest
    {
        public string EventName { get; set; }
        public EventArgument Argument { get; set; }
        public Action<EventResponse> OnSuccess { get; set; }
        public Action<EventResponse> OnFailure { get; set; }
        public EventExecutionType Type { get; set; }
    }

    public enum EventExecutionType
    {
        Single, // will fail if there are more than one event handlers
        First, // will use the first handler
        All //will use and return results from all of the handlers
    }
}
