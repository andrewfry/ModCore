﻿using System;
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

namespace ModCore.Www.Controllers
{
    public class HomeController : BaseController
    {
        private IThemeManager _themeManager;
        public HomeController(ILog log, ISessionManager sessionManager, ISiteSettingsManager siteSettingsManager,
            IBaseViewModelProvider baseModeProvider, IThemeManager themeManager)
            : base(log,sessionManager,siteSettingsManager, baseModeProvider)
        {
            _themeManager = themeManager;
        }

        public IActionResult Index()
        {
            var m = new BaseViewModel();
            
            this.CurrentSession.IsLoggedIn = true;
            this.CommitSession();
            var test = this.CurrentSession.IsLoggedIn;
            m.SiteSettings = new SiteSettingViewModel();
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
