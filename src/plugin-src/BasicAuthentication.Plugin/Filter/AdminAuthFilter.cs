using Microsoft.AspNetCore.Mvc.Filters;
using ModCore.Core.Controllers;
using System.Threading.Tasks;
using ModCore.Abstraction.Site;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using ModCore.Abstraction.Services.Access;
using ModCore.Models.Sessions;
using System;
using Microsoft.AspNetCore.Mvc;

namespace BasicAuthentication.Plugin.Filters
{
    public class AdminAuthFilter : AuthorizeAttribute, IAsyncAuthorizationFilter, IFilterMetadata
    {
        private readonly ISiteSettingsManagerAsync _siteSettings;
        private readonly ISessionService _sessionService;
        private readonly SessionData _currentSession;

        public AdminAuthFilter(ISiteSettingsManagerAsync siteSettings, ISessionService sessionService)
        {
            _siteSettings = siteSettings;
            _sessionService = sessionService;

            if(_sessionService == null)
            {
                throw new ArgumentNullException($"{nameof(_sessionService)} is null in Admin Auth Fitler");
            }

            _currentSession = _sessionService.GetCurrentOrDefault();

            if (_currentSession == null)
            {
                throw new ArgumentNullException($"{nameof(_currentSession)} is null in Admin Auth Fitler");
            }
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            if (context.RouteData.Values["Controller"].ToString().ToLower() == "account" && context.RouteData.Values["Action"].ToString().ToLower() == "login")
            {
                return;
            }

            if (_currentSession.IsLoggedIn)
            {
               return;
            }

            context.Result = new RedirectResult($"/Admin/Account/Login?ReturnUrl={context.HttpContext.Request.Path}");

        }

    }
}
