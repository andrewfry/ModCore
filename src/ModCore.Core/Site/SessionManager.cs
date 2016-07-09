using ModCore.Abstraction.Site;
using ModCore.Models.Sessions;
using System;

namespace ModCore.Core.Site
{
    public class SessionManager : ISessionManager
    {

        public SessionData CreateSession()
        {
            throw new NotImplementedException();
        }

        public bool KillSession()
        {
            throw new NotImplementedException();
        }

        public SessionData GetSession(string sessionId)
        {
            throw new NotImplementedException();
        }


        public bool IsValidToken(string sessionId, string token)
        {
            throw new NotImplementedException();
        }


    }
}
