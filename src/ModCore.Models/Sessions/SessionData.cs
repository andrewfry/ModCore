using ModCore.Models.Access;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModCore.Models.Sessions
{
    public class SessionData
    {

        public string SessionId { get; set; }

        public bool IsLoggedIn { get; set; }

        public string UserId { get; set; }

        public SessionUserData UserData { get; set; }


        public SessionData()
        {
            IsLoggedIn = false;
            UserData = null;
        }

    }
}
