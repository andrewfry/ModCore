using System;
using System.Collections.Generic;
using ModCore.Models.Access;
using System.Text;

namespace ModCore.ViewModels.Access
{
    public class vUserActivity
    {
        private string _fullRoute;

        public string Id { get; set; }

        public string FullRoute
        {
            get
            {
                if(string.IsNullOrEmpty(_fullRoute))
                {
                    var strBuilder = new StringBuilder();
                    foreach (var r in RouteValues)
                    {
                        strBuilder.AppendFormat("{0} : {1} ", r.Key, r.Value);
                    }

                    _fullRoute = strBuilder.ToString();
                }

                return _fullRoute;
            }
        }

        public Dictionary<string, string> RouteValues { get; set; }

        public string SessionId { get; set; }

        public string UserId { get; set; }

        public DateTime InsertDate { get; set; }

        public bool ErrorOccured { get; set; }

        public LifeCycle LifeCycle { get; set; }
    }
}
