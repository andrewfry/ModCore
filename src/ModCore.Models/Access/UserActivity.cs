using ModCore.Models.BaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModCore.Models.Access
{
    public class UserActivity : BaseEntity
    {

        public string ControllerName { get; set; }

        public string ActionName { get; set; }

        public Dictionary<string, string> RouteValues { get; set; }

        public string SessionId { get; set; }

        public string UserId { get; set; }

        public DateTime InsertDate { get; set; }

        public bool? ModelStateIsValid { get; set; }

        public ResultInfo Result { get; set; }

        public bool ErrorOccured { get; set; }

        public string ErrorMessage { get; set; }

        public LifeCycle LifeCycle { get; set; }


        public UserActivity()
        {
        }

    }

    public enum LifeCycle
    {
        ActionExecuting = 1,
        ActionExecuted = 2,
        ResultExecuted = 3,
        ResultExecuting = 4,
    }

    public enum ResultType
    {
        View = 1, 
        PartialView = 2,
        Json = 3,
        Redirect = 4,
        Other = 5
        
    }

    public class ResultInfo
    {
        public ResultType ResultType { get; set; }

        public string ViewName { get; set; }

        public string ModelType { get; set; }

        public object Model { get; set; }

        public int? StatusCode { get; set; }

        public string AdditionalInfo { get; set; }
    }
}
