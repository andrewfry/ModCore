using Microsoft.AspNetCore.Http;
using ModCore.Models.Sessions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ModCore.Abstraction.Site
{
    public interface IBaseController
    {
        SessionData CurrentSession { get; }

        ISessionManager SessionManager { get; }

        ISiteSettingsManager SiteSettingsManager { get; }


        HttpContext HttpContext { get; }

        HttpRequest Request { get; }

        ClaimsPrincipal User { get; }
    }
}
