using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ModCore.Core.Controllers;
using ModCore.Abstraction.Site;
using Microsoft.Extensions.Logging;
using ModCore.ViewModels.Base;
using ModCore.ViewModels.Admin.Themes;
using ModCore.Core.Themes;
using ModCore.Abstraction.Themes;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace ModCore.Www.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ThemeController : BaseController
    {
        private IThemeManager _themeManager; 
        public ThemeController(ILog log, ISessionManager sessionManager, ISiteSettingsManager siteSettingsManager,
            IBaseViewModelProvider baseModeProvider, IThemeManager themeManager)
            : base(log,sessionManager,siteSettingsManager, baseModeProvider)
        {
            _themeManager = themeManager;
        }

        public IActionResult Index()
        {
            var themeList = new vThemeList();
            themeList.ThemeList = _themeManager.AvailableThemes.Select(
                a => new vTheme
                {
                    ThemeName = a.ThemeName,
                    ThemeVersion = a.ThemeVersion,
                    Description = a.Description,
                    DisplayName = a.DisplayName
                }

            ).ToList();
            return View(themeList);
        }
    }
}
