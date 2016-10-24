using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ModCore.Core.Controllers;
using ModCore.Abstraction.Site;
using Microsoft.Extensions.Logging;
using ModCore.ViewModels.Base;
using ModCore.Models.Sessions;
using Microsoft.AspNetCore.Authorization;

namespace ModCore.Www.Controllers
{
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
            
            this.CurrentSession.IsLoggedIn = true;
            this.CommitSession();

            var test = this.CurrentSession.IsLoggedIn;

            return View(m);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
