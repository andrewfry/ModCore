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
using ModCore.Abstraction.Themes;
using ModCore.Core.Themes;
using ModCore.Models.Page;

namespace ModCore.Www.Controllers
{
    public class PageController : BaseController
    {
        private IThemeManager _themeManager;
        public PageController(ILog log, ISessionManager sessionManager, ISiteSettingsManager siteSettingsManager,
            IBaseViewModelProvider baseModeProvider, IThemeManager themeManager)
            : base(log, sessionManager, siteSettingsManager, baseModeProvider)
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
            m.SiteSettings.Theme = new vTheme() {
                ThemeName = _themeManager.ActiveTheme.ThemeName,
                Description = _themeManager.ActiveTheme.Description,
                DisplayName = _themeManager.ActiveTheme.DisplayName,
                CSSLocation = "/Themes/" + _themeManager.ActiveTheme.ThemeName + "/style.css"
            
            };
            var content = RouteData.Values["page"] as Page;
            ViewBag.Content = content.HTMLContent;
            return View(m);
        }
    }
}
