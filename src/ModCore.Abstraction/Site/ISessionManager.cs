using ModCore.Models.Sessions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModCore.Abstraction.Site
{
    public interface ISessionManager
    {
        SessionData CreateSession();

        bool KillSession();

        SessionData GetSession(string sessionId);
        
        bool IsValidToken(string sessionId, string token);
    }
}
