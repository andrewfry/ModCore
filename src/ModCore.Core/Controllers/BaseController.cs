using System;
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


namespace ModCore.Core.Controllers
{
    [NamespaceConstraint]
    public class BaseController : Controller, IBaseController
    {
        protected ILog _logger;
        private SessionData _currentSession;
        private ISession _session => HttpContext.Session;
        private ISiteSettingsManager _siteSettingsManager;
        private IBaseViewModelProvider _baseClassProvider;

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
                    var jsonString = _session.GetString("sessionData");
                    if (string.IsNullOrEmpty(jsonString))

                    {
                        var newSession = new SessionData();
                        newSession.SessionId = HttpContext.Session.Id;

                        jsonString = newSession.ToJson();
                        _session.SetString("sessionData", jsonString);

                        _currentSession = newSession;

                        return _currentSession;
                    }

                    _currentSession = jsonString.ToObject<SessionData>();
                }

                return _currentSession;
            }

        }

        public ISiteSettingsManager SiteSettingsManager
        {
            get
            {
                return _siteSettingsManager;
            }
        }

        public BaseController(ILog log, ISessionManager sessionManager, ISiteSettingsManager siteSettingsManager,
            IBaseViewModelProvider baseClassProvider)
        {
            _logger = log;
            _siteSettingsManager = siteSettingsManager;
            _baseClassProvider = baseClassProvider;
        }

        public void CommitSession()
        {
            var jsonString = _currentSession.ToJson();
            _session.SetString("sessionData", jsonString);

            _currentSession = null;
        }

        public override ViewResult View(string viewName, object model)
        {
            return View(viewName, model, true);
        }

        public ViewResult View(string viewName, object model, bool updateBaseViewModel)
        {
            var baseVm = model as BaseViewModel;
            if (model == null)
                throw new ArgumentNullException("You must provide a model to every view that inherits from BaseViewModel");

            if (updateBaseViewModel)
                _baseClassProvider.Update(this, baseVm);

            return base.View(viewName, model);
        }

        public string RenderViewAsString(string viewPath)
        {
            return RenderViewAsString(viewPath, "");
        }

        public string RenderViewAsString<TModel>(string viewPath, TModel model)
        {
           var engine = ControllerContext.HttpContext.RequestServices.GetService(typeof(ICompositeViewEngine)) as ICompositeViewEngine;
           var viewEngineResult = engine.GetView("~/", viewPath, false);

            if (!viewEngineResult.Success)
            {
                throw new InvalidOperationException($"Could not find view {viewPath}");
            }

            var view = viewEngineResult.View;
            var result = "";
            using (var output = new StringWriter())
            {
                var viewContext = new ViewContext();
                viewContext.HttpContext = ControllerContext.HttpContext; // _httpContextAccessor.HttpContext;
                viewContext.ViewData = new ViewDataDictionary<TModel>(new EmptyModelMetadataProvider(), new ModelStateDictionary())
                { Model = model };
                viewContext.Writer = output;

                view.RenderAsync(viewContext).GetAwaiter().GetResult();

                result = output.GetStringBuilder().Replace("\"", "'").ToString();

            }

            return result;
        }
    }
}
