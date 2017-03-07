using Microsoft.AspNetCore.Mvc;
using ModCore.Core.Controllers;
using ModCore.Abstraction.Site;
using ModCore.ViewModels.Base;
using ModCore.ViewModels.Theme;
using ModCore.Abstraction.Themes;
using AutoMapper;
using ModCore.Abstraction.Services.Access;
using ModCore.Core.Site;
using ModCore.ViewModels.Site;
using System.Threading.Tasks;
using ModCore.Models.Site;
using System.Collections.Generic;

namespace ModCore.Www.Components

{
    public class MenuComponent : ViewComponent
    {
        private IMenuManager _menuManager;
        private ILog _log;
        private IMapper _mapper;

        public MenuComponent(ILog log, IMapper mapper, IMenuManager menuManager)
        {
            _log = log;
            _mapper = mapper;
            _menuManager = menuManager;
        }


        public async Task<IViewComponentResult> InvokeAsync(string menuName)
        {
            var menu = _mapper.Map<vMenu>(_menuManager.GetMenuByName(menuName));

            return View("Default", menu);
        }
      
    }
}
