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
using RoleBasedPermission.Plugin.Abstraction;
using ModCore.Models.Access;
using ModCore.Abstraction.Services.Site;
using ModCore.Abstraction.Plugins;

namespace RoleBasedPermission.Plugin.Filters
{
    public class PermissionFilter : ActionFilterAttribute
    {

        public PermissionFilter()
        {
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {

            var controller = context.Controller as BaseController;

            if (controller.CurrentSession.IsLoggedIn)
            {

                var permService = context.HttpContext.RequestServices.GetService(typeof(IPermissionService)) as IPermissionService;
                var userRoles = controller.CurrentSession.UserData.RoleIds;
                var permResult = permService.CheckPermission(controller, context.ActionDescriptor, userRoles);

                var logger = context.HttpContext.RequestServices.GetService(typeof(IPluginLog)) as IPluginLog;
                logger.SetPlugin(new RoleBasedPermission());

                if (permResult.ErrorOccured)
                {
                    logger.LogError<PermissionFilter>(permResult.Exception, "Error occured while authorizing user id {0} with controller '{1}' and action '{2}'", controller.CurrentSession.UserId, controller.ControllerContext.ActionDescriptor.ControllerName, context.ActionDescriptor.DisplayName);
                }

                if (!(permResult.PermissionExecutedResult == PermissionExecutedResult.Granted))
                {
                    logger.LogDebug<PermissionFilter>("User denied access. User id {0} for controller '{1}' and action '{2}'", controller.CurrentSession.UserId, controller.ControllerContext.ActionDescriptor.ControllerName, context.ActionDescriptor.DisplayName);
                    context.Result = controller.RedirectToAction("Error", new { id = "404" });
                }

                await next();
                return;
            }

        }
    }
}
