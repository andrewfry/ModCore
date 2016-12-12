using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ModCore.Core.Controllers;
using ModCore.Core.HelperExtensions;
using ModCore.Abstraction.Site;
using Microsoft.Extensions.Logging;
using ModCore.ViewModels.Base;
using ModCore.ViewModels.Theme;
using ModCore.Abstraction.Themes;
using AutoMapper;
using ModCore.Abstraction.Services.Access;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace ModCore.Www.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ThemeController : BaseController
    {
        private IThemeManager _themeManager;
        public ThemeController(ILog log, ISiteSettingsManagerAsync siteSettingsManager,
            IBaseViewModelProvider baseModeProvider, IThemeManager themeManager, IMapper mapper, ISessionService sessionService)
            : base(log,  siteSettingsManager, baseModeProvider, mapper, sessionService)
        {
            _themeManager = themeManager;
        }

        public IActionResult Index()
        {
            var themeList = GetThemeList();
            return View(themeList);
        }

        public JsonResult SetTheme(string themeName)
        {
            var theme = _themeManager.AvailableThemes.Where(a => a.ThemeName.ToLower() == themeName.ToLower()).SingleOrDefault();
            if (theme != null)
            {
                _themeManager.ActivateTheme(theme);
            }

            var themeList = GetThemeList().ThemeList;

            string view = this.RenderViewAsString("Areas/Admin/Views/Theme/_ThemeList.cshtml", themeList);
            return Json(new { html = view });
        }

        private vThemeList GetThemeList()
        {
            var themeList = new vThemeList();
            themeList.ThemeList = _themeManager.AvailableThemes.Select(
                    a => new vTheme
                    {
                        ThemeName = a.ThemeName,
                        ThemeVersion = a.ThemeVersion,
                        Description = a.Description,
                        DisplayName = a.DisplayName,
                        Active = a.ThemeName == _themeManager.ActiveTheme.ThemeName ? true : false
                    }

                ).ToList();

            return themeList;
        }
    }
}
