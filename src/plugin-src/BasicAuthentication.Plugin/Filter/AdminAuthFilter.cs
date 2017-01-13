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
using ModCore.Abstraction.Plugins;
using BasicAuthentication.Plugin;

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

            if (_sessionService == null)
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
            var settings = context.HttpContext.RequestServices.GetService(typeof(IPluginSettingsManager)) as IPluginSettingsManager;
            settings.SetPlugin(new BasicAuthentication());

            var isEnabled = await settings.GetSettingAsync<bool>(BasicAuthentication.BuiltInSettings.Enabled);
            if (isEnabled == false)
                return;

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
