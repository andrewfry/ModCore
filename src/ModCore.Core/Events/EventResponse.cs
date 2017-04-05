using System;
using System.Collections.Generic;
using System.Text;

namespace ModCore.Core.Events
{
    public class EventResponse
    {
        public object Value { get; set; }
        public bool Success { get; set; }
        public List<string> HandledBy { get; set; }
        public Exception Exception { get; set; }
    }
}
