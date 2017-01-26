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
    public class MenuController : BaseController
    {
        private IMenuManager _menuManager;

        public MenuController(ILog log, ISiteSettingsManagerAsync siteSettingsManager,
            IBaseViewModelProvider baseModeProvider,  IMapper mapper, ISessionService sessionService, IMenuManager menuManager)
            : base(log,siteSettingsManager, baseModeProvider, mapper, sessionService)
        {
            _menuManager = menuManager;
        }

        public IActionResult RenderMenu(string menuName)
        {
            var m = new BaseViewModel();
            
        
            return View(m);
        }

        
    }
}
