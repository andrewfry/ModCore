using Microsoft.AspNetCore.Http;
using ModCore.Models.Sessions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModCore.Abstraction.Services.Access
{
    public interface ISessionService
    {
        SessionData GetCurrentOrDefault();

        void Discard();

        void Commit(SessionData session);
    }
}
