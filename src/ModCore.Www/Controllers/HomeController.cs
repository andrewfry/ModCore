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
using ModCore.ViewModels.Theme;
using ModCore.Core.Themes;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using ModCore.Abstraction.Themes;
using AutoMapper;

namespace ModCore.Www.Controllers
{
    public class HomeController : BaseController
    {
        private IThemeManager _themeManager;
        public HomeController(ILog log, ISiteSettingsManagerAsync siteSettingsManager,
            IBaseViewModelProvider baseModeProvider, IThemeManager themeManager, IMapper mapper)
            : base(log,siteSettingsManager, baseModeProvider, mapper)
        {
            _themeManager = themeManager;
        }

        public IActionResult Index()
        {
            var m = new BaseViewModel();
            
           m.SiteSettings = new vSiteSettings();
            m.SiteSettings.Theme = new vTheme()
            {
                ThemeName = _themeManager.ActiveTheme.ThemeName,
                Description = _themeManager.ActiveTheme.Description,
                DisplayName = _themeManager.ActiveTheme.DisplayName,
                CSSLocation = "/Themes/" + _themeManager.ActiveTheme.ThemeName + "/style.css"

            };
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
