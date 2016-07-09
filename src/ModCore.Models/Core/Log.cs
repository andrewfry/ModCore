using ModCore.Models.Enum;
using ModCore.Models.Sessions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModCore.Models.Core
{
    public class Log
    {
        public string ClassName { get; set; }

        public string PluginName { get; set; }

        public ErrorLevel ErrorLevel { get; set; }

        public string Message { get; set; }

        public string ErrorMessage { get; set; }

        public string InnerException { get; set; }

        public string StackTrace { get; set; }

        public SessionData Session { get; set; }
    }
}
