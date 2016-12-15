using ModCore.Models.BaseEntities;
using ModCore.Models.Enum;
using ModCore.Models.Sessions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ModCore.Models.Access;
using System.Text;

namespace ModCore.ViewModels.Access
{
    public class vUserActivityDetailed : vUserActivity
    {
        public string ErrorMessage { get; set; }

        public bool? ModelStateIsValid { get; set; }

        public string ViewName { get; set; }

        public string ModelType { get; set; }

        public string ResultType { get; set; }

        public int? StatusCode { get; set; }

        public string AdditionalInfo { get; set; }
    }
}
