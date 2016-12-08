using Microsoft.AspNetCore.Mvc.Filters;
using ModCore.Core.Controllers;
using System.Threading.Tasks;
using ModCore.Abstraction.Site;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Authorization;

namespace BasicAuthentication.Plugin.Filters
{
    public class AdminAuthFilter2 : IAsyncAuthorizationFilter, IFilterMetadata
    {
        private readonly ISiteSettingsManagerAsync _siteSettings;

        public AdminAuthFilter2(ISiteSettingsManagerAsync siteSettings, AuthorizationPolicy policy)
        {
            _siteSettings = siteSettings;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            //if (context.RouteData.Values["Area"] == null)
            //{
            //    return Task.FromResult(0);
            //}

            //var isAdminRoute = string.Compare(context.RouteData.Values["Area"].ToString(), "Admin", true) == 0;
            //if (!isAdminRoute)
            //{
            //    return Task.FromResult(0);
            //}

            //if (context.RouteData.Values["Controller"].ToString().ToLower() == "account" && context.RouteData.Values["Action"].ToString().ToLower() == "login")
            //{
            //    return Task.FromResult(0);
            //}

            //var controller = context. as BaseController;

            //if (controller.CurrentSession.IsLoggedIn)
            //{
            //    var authService = context.HttpContext.RequestServices.GetService(typeof(IAuthenticationService)) as IAuthenticationService;
            //    var userId = controller.CurrentSession.UserId;


            //    var routeData = context.RouteData;
            //    //var isAllowed = await authService.UserAllowedAdminAccess(userId, routeData);
            //    var isAllowed = true;

            //    if (!isAllowed)
            //    {
            //        context.Result = controller.RedirectToAction("Error", new { id = "404" });
            //    }

            //    await next();
            //    return;
            //}

            //context.HttpContext.Response.Redirect($"/Admin/Account/Login?ReturnUrl={context.HttpContext.Request.Path}");
        }

    }
}
