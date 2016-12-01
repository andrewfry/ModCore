using Microsoft.AspNetCore.Mvc.Filters;
using ModCore.Core.Controllers;
using System.Threading.Tasks;
using ModCore.Abstraction.Site;

namespace BasicAuthentication.Plugin.Filters
{
    public class AdminAuthFilter : ActionFilterAttribute
    {
        private readonly ISiteSettingsManagerAsync _siteSettings;

        public AdminAuthFilter(ISiteSettingsManagerAsync siteSettings)
        {
            _siteSettings = siteSettings;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.RouteData.Values["Area"] == null)
            {
                await next();
                return;
            }

            var isAdminRoute = string.Compare(context.RouteData.Values["Area"].ToString(), "Admin", true) == 0;
            if (!isAdminRoute)
            {
                await next();
                return;
            }

            if (context.RouteData.Values["Controller"].ToString().ToLower() == "account" && context.RouteData.Values["Action"].ToString().ToLower() == "login")
            {
                await next();
                return;
            }

            var controller = context.Controller as BaseController;

            if (controller.CurrentSession.IsLoggedIn)
            {
                var authService = context.HttpContext.RequestServices.GetService(typeof(IAuthenticationService)) as IAuthenticationService;
                var userId = controller.CurrentSession.UserId;


                var routeData = context.RouteData;
                //var isAllowed = await authService.UserAllowedAdminAccess(userId, routeData);
                var isAllowed = true;

                if (!isAllowed)
                {
                    context.Result = controller.RedirectToAction("Error", new { id = "404" });
                }

                await next();
                return;
            }

            context.HttpContext.Response.Redirect($"/Admin/Account/Login?ReturnUrl={context.HttpContext.Request.Path}");
        }
    }
}
