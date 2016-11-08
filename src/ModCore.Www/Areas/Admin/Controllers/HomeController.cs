using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ModCore.Core.Controllers;
using ModCore.Abstraction.Site;
using Microsoft.Extensions.Logging;
using ModCore.ViewModels.Base;
using Microsoft.AspNetCore.Authorization;

namespace ModCore.Www.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class HomeController : BaseController
    {
        public HomeController(ILog log, ISessionManager sessionManager, ISiteSettingsManager siteSettingsManager,
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
