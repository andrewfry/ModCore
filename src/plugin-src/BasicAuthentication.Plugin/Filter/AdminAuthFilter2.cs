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

namespace BasicAuthentication.Plugin.Filters
{
    public class AdminAuthFilter2 : AuthorizeAttribute, IAsyncAuthorizationFilter, IFilterMetadata
    {
        private readonly ISiteSettingsManagerAsync _siteSettings;
        private readonly ISessionService _sessionService;
        private readonly SessionData _currentSession;

        public AdminAuthFilter2(ISiteSettingsManagerAsync siteSettings, ISessionService sessionService)
        {
            _siteSettings = siteSettings;
            _sessionService = sessionService;

            if(_sessionService == null)
            {
                throw new ArgumentNullException($"{nameof(_sessionService)} is null in Admin Auth Fitler");
            }

            _currentSession = _sessionService.GetCurrentOrDefault();
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            if (context.RouteData.Values["Area"] == null)
            {
                return;
            }

            var isAdminRoute = string.Compare(context.RouteData.Values["Area"].ToString(), "Admin", true) == 0;
            if (!isAdminRoute)
            {
                return;
            }

            if (context.RouteData.Values["Controller"].ToString().ToLower() == "account" && context.RouteData.Values["Action"].ToString().ToLower() == "login")
            {
                return;
            }

            var controllerDesc = context.ActionDescriptor as ControllerActionDescriptor;

            if (_currentSession.IsLoggedIn)
            {
                var authService = context.HttpContext.RequestServices.GetService(typeof(IAuthenticationService)) as IAuthenticationService;
                var userId = _currentSession.UserId;


                var routeData = context.RouteData;
                //var isAllowed = await authService.UserAllowedAdminAccess(userId, routeData);
                var isAllowed = true;

                if (!isAllowed)
                {
                    context.HttpContext.Response.Redirect("/Error/404"); 
                }

                return;
            }

            context.HttpContext.Response.Redirect($"/Admin/Account/Login?ReturnUrl={context.HttpContext.Request.Path}");
        }

    }
}
