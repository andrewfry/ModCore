using Microsoft.AspNetCore.Mvc;
using ModCore.Core.Controllers;
using ModCore.Abstraction.Site;
using ModCore.ViewModels.Base;
using ModCore.ViewModels.Theme;
using ModCore.Abstraction.Themes;
using AutoMapper;
using ModCore.Abstraction.Services.Access;

namespace ModCore.Www.Controllers
{
    public class HomeController : BaseController
    {
        private IThemeManager _themeManager;
        public HomeController(ILog log, ISiteSettingsManagerAsync siteSettingsManager,
            IBaseViewModelProvider baseModeProvider, IThemeManager themeManager, IMapper mapper, ISessionService sessionService)
            : base(log,siteSettingsManager, baseModeProvider, mapper, sessionService)
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
