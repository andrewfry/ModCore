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
    public class MenuItemComponent : ViewComponent
    {
        private IMenuManager _menuManager;
        private ILog _log;
        private IMapper _mapper;

        public MenuItemComponent(ILog log, IMapper mapper, IMenuManager menuManager)
        {
            _log = log;
            _mapper = mapper;
            _menuManager = menuManager;
        }


        public async Task<IViewComponentResult> InvokeAsync(List<MenuItem> menuItems)
        {
            return View("MenuItems", menuItems);
        }
    }
}
