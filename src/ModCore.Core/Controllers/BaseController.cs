using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ModCore.Abstraction.Site;
using ModCore.Models.Sessions;
using ModCore.Core.Constraints;

namespace ModCore.Core.Controllers
{
    [NamespaceConstraint]
    public class BaseController : Controller
    {
        protected ILog _logger;
        private SessionData _currentSession;
        private ISessionManager _sessionManager;
        private ISiteSettingsManager _siteSettingsManager;

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

        public BaseController(ILog log, ISessionManager sessionManager, ISiteSettingsManager siteSettingsManager)
        {
            _logger = log;
            _sessionManager = sessionManager;
            _siteSettingsManager = siteSettingsManager;
        }

    }
}
