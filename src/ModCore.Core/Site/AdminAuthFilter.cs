using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using ModCore.Abstraction.Services.Access;
using ModCore.Core.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModCore.Core.Site
{



    public class AdminAuthFilter : ActionFilterAttribute
    {


        public AdminAuthFilter()

        {

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
                var userService = context.HttpContext.RequestServices.GetService(typeof(IUserService)) as IUserService;
                var userId = controller.CurrentSession.UserId;
                //var userId = context.HttpContext.User.Claims.Single(a => a.Type == "UserId").Value;

                //if (string.IsNullOrEmpty(controller.CurrentSession.UserId))
                //{
                //    var user = await userService.GetByIdAsync(userId);

                //    controller.CurrentSession.UpdateUserData(user, true);
                //    controller.CommitSession();
                //}

                var routeData = context.RouteData;
                var isAllowed = await userService.UserAllowedAdminAccess(userId, routeData);

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
