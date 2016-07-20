using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ModCore.Core.Controllers;
using ModCore.Abstraction.Site;
using Microsoft.Extensions.Logging;
using ModCore.ViewModels.Base;

namespace ModCore.Www.Controllers
{
    public class AdminController : BaseController
    {
        public AdminController(ILog log, ISessionManager sessionManager, ISiteSettingsManager siteSettingsManager,
            IBaseViewModelProvider baseModeProvider)
            : base(log,sessionManager,siteSettingsManager, baseModeProvider)
        {

        }

        public IActionResult Index()
        {
            var m = new BaseViewModel();


            return View(m);
        }

    }
}
