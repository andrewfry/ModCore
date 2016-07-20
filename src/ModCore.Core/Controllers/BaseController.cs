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

namespace ModCore.Core.Controllers
{
    [NamespaceConstraint]
    public class BaseController : Controller, IBaseController
    {
        protected ILog _logger;
        private SessionData _currentSession;
        private ISessionManager _sessionManager;
        private ISiteSettingsManager _siteSettingsManager;
        private IBaseViewModelProvider _baseClassProvider;

        public SessionData CurrentSession
        {
            get
            {
                return _currentSession;
            }
        }

        public ISessionManager SessionManager
        {
            get
            {
                return _sessionManager;
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
            _sessionManager = sessionManager;
            _siteSettingsManager = siteSettingsManager;
            _baseClassProvider = baseClassProvider;
        }


        public override ViewResult View(string viewName, object model)
        {
            var baseVm = model as BaseViewModel;
            if (model == null)
                throw new ArgumentNullException("You must provide a model to every view that inherits from BaseViewModel");

            _baseClassProvider.Update(this, baseVm);

            return base.View(viewName, model);
        }

    }
}
