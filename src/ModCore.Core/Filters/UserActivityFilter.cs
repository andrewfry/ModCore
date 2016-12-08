using Microsoft.AspNetCore.Mvc.Filters;
using ModCore.Core.Controllers;
using System.Threading.Tasks;
using ModCore.Abstraction.Site;
using System.Linq;
using ModCore.Models.Access;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using ModCore.Core.Site;
using ModCore.Abstraction.Services.Access;

namespace ModCore.Core.Filters
{
    public class UserActivityFilter : ActionFilterAttribute
    {
        private readonly ISiteSettingsManagerAsync _siteSettings;
        private readonly IUserActivityService _activityService;

        public UserActivityFilter(ISiteSettingsManagerAsync siteSettings, IUserActivityService service)
        {
            _siteSettings = siteSettings;
            _activityService = service;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {


            base.OnActionExecuting(context);
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            var trackingActivity = _siteSettings.GetSettingAsync<bool>(BuiltInSettings.UsrActTrking).Result;
            if (!trackingActivity)
                return;

            var trackingActivityDetailed = _siteSettings.GetSettingAsync<bool>(BuiltInSettings.UsrActTrkingDetailed).Result;

            var baseController = context.Controller as BaseController;
            var usrActivity = new UserActivity
            {
                ControllerName = baseController.GetType().FullName,
                ActionName = context.ActionDescriptor.DisplayName,
                ModelStateIsValid = context.ModelState.IsValid,
                RouteValues = trackingActivityDetailed ? context.ActionDescriptor.RouteValues as Dictionary<string, string> : null,
                SessionId = baseController.CurrentSession.SessionId,
                UserId = baseController.CurrentSession.UserId,
                Result = null,
                ErrorOccured = context.ExceptionHandled,
                ErrorMessage = context.Exception?.Message,
                LifeCycle = LifeCycle.ActionExecuted
            };

            _activityService.AddActivity(usrActivity);

            base.OnActionExecuted(context);
        }

        public override void OnResultExecuting(ResultExecutingContext context)
        {
            base.OnResultExecuting(context);
        }

        public override void OnResultExecuted(ResultExecutedContext context)
        {
            var trackingActivity = _siteSettings.GetSettingAsync<bool>(BuiltInSettings.UsrActTrking).Result;
            if (!trackingActivity)
                return;

            var trackingActivityDetailed = _siteSettings.GetSettingAsync<bool>(BuiltInSettings.UsrActTrkingDetailed).Result;
            var trackBaseModel = _siteSettings.GetSettingAsync<bool>(BuiltInSettings.UsrActTrkingBaseModelRecord).Result;

            var baseController = context.Controller as BaseController;
            var usrActivity = new UserActivity
            {
                ControllerName = baseController.GetType().FullName,
                ActionName = context.ActionDescriptor.DisplayName,
                ModelStateIsValid = context.ModelState.IsValid,
                RouteValues = trackingActivityDetailed ? context.ActionDescriptor.RouteValues as Dictionary<string, string> : null,
                SessionId = baseController.CurrentSession.SessionId,
                UserId = baseController.CurrentSession.UserId,
                Result = GetResultInfo(context, trackingActivityDetailed, trackBaseModel),
                LifeCycle = LifeCycle.ResultExecuted
            };

            _activityService.AddActivity(usrActivity);

            base.OnResultExecuted(context);
        }


        private ResultInfo GetResultInfo(ResultExecutedContext context, bool detailedLogging, bool recordBaseModel)
        {
            var returnObj = new ResultInfo();
            var result = context.Result;

            if (result as ViewResult != null)
            {
                var viewResult = result as ViewResult;

                returnObj.ResultType = ResultType.View;
                returnObj.ViewName = viewResult.ViewName;
                if (detailedLogging && (recordBaseModel && returnObj.Model.GetType().Name != "BaseViewModel"))
                    returnObj.Model = viewResult.Model;
                returnObj.StatusCode = viewResult.StatusCode;
                returnObj.ModelType = viewResult.Model?.GetType().FullName;
            }
            else if (result as JsonResult != null)
            {
                var jsonResult = result as JsonResult;
                returnObj.ResultType = ResultType.Json;
                if (detailedLogging && (recordBaseModel && returnObj.Model.GetType().Name != "BaseViewModel"))
                    returnObj.Model = jsonResult.Value;
                returnObj.StatusCode = jsonResult.StatusCode;
            }
            else if (result as PartialViewResult != null)
            {
                var viewResult = result as PartialViewResult;

                returnObj.ResultType = ResultType.View;
                returnObj.ViewName = viewResult.ViewName;
                if (detailedLogging && (recordBaseModel && returnObj.Model.GetType().Name != "BaseViewModel"))
                    returnObj.Model = viewResult.Model;
                returnObj.StatusCode = viewResult.StatusCode;
                returnObj.ModelType = viewResult.Model?.GetType().FullName;
            }
            else if (result as RedirectResult != null)
            {
                var redResult = result as RedirectResult;

                returnObj.ResultType = ResultType.Redirect;
                returnObj.AdditionalInfo = redResult.Url;
            }
            else
            {
                returnObj.ResultType = ResultType.Other;
                returnObj.AdditionalInfo = $"ResultType of {result.GetType().FullName}.";
            }


            return returnObj;
        }

    }



}
