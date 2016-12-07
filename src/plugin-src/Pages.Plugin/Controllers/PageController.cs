using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ModCore.Core.Controllers;
using ModCore.Abstraction.Plugins;
using ModCore.Abstraction.Site;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ModCore.ViewModels.Base;
using Pages.Plugin.ViewModels;
using Pages.Plugin.Services;

namespace Pages.Plugin.Controllers
{
    public class PageController : BasePluginController
    {
        public override IPlugin Plugin
        {
            get
            {
                return new Pages();
            }
        }
        private IPageService _pageService;
        public PageController(IPluginLog log,ISiteSettingsManagerAsync siteSettingsManager, IPluginSettingsManager pluginSettingsManager,
            IBaseViewModelProvider baseViewModelProvider, IMapper mapper, IPageService pageService) :
            base(log, siteSettingsManager, pluginSettingsManager, baseViewModelProvider, mapper)
        {
            _pageService = pageService;
        }

        public async Task<IActionResult> Index()
        {
            var m = new PageListViewModel();
         
            m.PageList = await _pageService.PageList();
            return View(m);
        }

        public IActionResult Test()
        {
            var m = new BaseViewModel();
            return View(m);
        }

    }
}
