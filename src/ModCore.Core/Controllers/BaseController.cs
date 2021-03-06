﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ModCore.Abstraction.Site;
using ModCore.Models.Sessions;
using ModCore.Core.Constraints;
using ModCore.ViewModels.Base;
using Microsoft.AspNetCore.Http;
using ModCore.Core.HelperExtensions;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using System.IO;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using AutoMapper;
using ModCore.Utilities.HelperExtensions;
using ModCore.Abstraction.Services.Access;
using Microsoft.AspNetCore.Mvc.Abstractions;
using ModCore.Core.Site;
using Newtonsoft.Json;
using System.Net;

namespace ModCore.Core.Controllers
{
    [NamespaceConstraint]
    public class BaseController : Controller, IBaseController
    {
        protected ILog _logger;
        private SessionData _currentSession;
        private ISession _session => HttpContext.Session;
        private ISiteSettingsManagerAsync _siteSettingsManager;
        private IBaseViewModelProvider _baseClassProvider;
        private ISessionService _sessionService;
        protected IMapper _mapper;

        public SessionData CurrentSession
        {
            get
            {
                if (_session == null)
                {
                    throw new NullReferenceException("The session is null for the CurrentSession property in the BaseController");
                }

                if (_currentSession == null)
                {
                    _currentSession = _sessionService.GetCurrentOrDefault();
                }

                return _currentSession;
            }

        }

        public ISiteSettingsManagerAsync SiteSettingsManager
        {
            get
            {
                return _siteSettingsManager;
            }
        }

        public BaseController(ILog log, ISiteSettingsManagerAsync siteSettingsManager,
            IBaseViewModelProvider baseClassProvider, IMapper mapper, ISessionService sessionService)
        {
            _logger = log;
            _siteSettingsManager = siteSettingsManager;
            _baseClassProvider = baseClassProvider;
            _mapper = mapper;
            _sessionService = sessionService;
        }


        public IActionResult ReturnError(HttpStatusCode code)
        {
                return RedirectToAction("Error", new { code = code });
        }

        [NonAction]
        public void DiscardSession()
        {
            // _session.SetString("sessionData", null);
            _sessionService.Discard();
            _currentSession = null;
        }

        [NonAction]
        public void CommitSession()
        {
            // var jsonString = _currentSession.ToJson();
            //_session.SetString("sessionData", jsonString);
            _sessionService.Commit(_currentSession);
            _currentSession = null;
        }

        [NonAction]
        public override ViewResult View(string viewName, object model)
        {
            return View(viewName, model, true);
        }

        [NonAction]
        public ViewResult View(string viewName, object model, bool updateBaseViewModel)
        {
            var baseVm = model as BaseViewModel;
            if (baseVm == null)
                throw new ArgumentNullException(nameof(model), "You must provide a model to every view that inherits from BaseViewModel");

            if (updateBaseViewModel)
                _baseClassProvider.Update(this, baseVm);

            return base.View(viewName, model);
        }

        [NonAction]
        public Task<string> RenderViewAsString(string viewPath)
        {
            return RenderViewAsString(viewPath, "");
        }

        [NonAction]
        public JsonResult JsonSuccess(object value, JsonSerializerSettings settings)
        {
            return Json(new JsonSuccessResult(value), settings);
        }

        [NonAction]
        public JsonResult JsonSuccess(object value)
        {
            return Json(new JsonSuccessResult(value));
        }

        [NonAction]
        public JsonResult JsonSuccess()
        {
            return Json(new JsonSuccessResult(null));
        }

        [NonAction]
        public JsonResult JsonFail(object value, JsonSerializerSettings settings)
        {
            return Json(new JsonFailResult(value), settings);
        }

        [NonAction]
        public JsonResult JsonFail(object value)
        {
            return Json(new JsonFailResult(value));
        }

        [NonAction]
        public JsonResult JsonFail(params string[] errorMessages)
        {
            return Json(new JsonFailResult(null, errorMessages));
        }

        [NonAction]
        public JsonResult JsonFail()
        {
            return Json(new JsonFailResult(null));
        }

        [NonAction]
        public async Task<string> RenderViewAsString(string viewName, object model)
        {
            var engine = ControllerContext.HttpContext.RequestServices.GetService(typeof(ICompositeViewEngine)) as ICompositeViewEngine;
            var tempDataProvider = ControllerContext.HttpContext.RequestServices.GetService(typeof(ITempDataProvider)) as ITempDataProvider;
            var actionContext = new ActionContext(HttpContext, RouteData, new ActionDescriptor());

            using (var sw = new StringWriter())
            {
                var viewResult = engine.FindView(actionContext, viewName, false);

                if (viewResult.View == null)
                {
                    throw new ArgumentNullException($"{viewName} does not match any available view");
                }

                var viewDictionary = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
                {
                    Model = model
                };

                var viewContext = new ViewContext(
                    actionContext,
                    viewResult.View,
                    viewDictionary,
                    new TempDataDictionary(actionContext.HttpContext, tempDataProvider),
                    sw,
                    new HtmlHelperOptions()
                );

                await viewResult.View.RenderAsync(viewContext);
                return sw.ToString();
            }
        }

    }
}
