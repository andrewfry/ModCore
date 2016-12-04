using ModCore.Models.BaseEntities;
using ModCore.Models.Enum;
using ModCore.Models.Sessions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace ModCore.ViewModels.Core
{
    public class vLog
    {
        public string Id { get; set; }

        public string Route { get; set; }

        public string ClassName { get; set; }

        public string PluginName { get; set; }

        public LogLevel ErrorLevel { get; set; }

        public string Message { get; set; }

        public string ErrorMessage { get; set; }

        public string InnerException { get; set; }

        public string StackTrace { get; set; }

        public DateTime InsertDate { get; set; }
    }
}
