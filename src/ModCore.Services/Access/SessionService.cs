using Microsoft.AspNetCore.Http;
using ModCore.Abstraction.Services.Access;
using ModCore.Models.Sessions;
using ModCore.Utilities.HelperExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModCore.Services.Access
{
    public class SessionService : ISessionService
    {
        private ISession _session;

        public SessionService(ISession session)
        {
            _session = session;

            if (_session == null)
            {
                throw new NullReferenceException("The session is null for the CurrentSession property in the SessionService");
            }
        }


        public SessionData GetCurrentOrDefault()
        {
            var _currentSession = new SessionData();
            var jsonString = _session.GetString("sessionData");
            if (string.IsNullOrEmpty(jsonString))

            {
                var newSession = new SessionData();
                newSession.SessionId = _session.Id;

                jsonString = newSession.ToJson();
                _session.SetString("sessionData", jsonString);

                _currentSession = newSession;

                return _currentSession;
            }

            _currentSession = jsonString.ToObject<SessionData>();

            return _currentSession;
        }

        public void Discard()
        {
            _session.SetString("sessionData", null);
        }

        public void Commit(SessionData session)
        {
            var jsonString = session.ToJson();
            _session.SetString("sessionData", jsonString);
        }

    }
}
