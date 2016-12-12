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
using ModCore.Abstraction.Services.Access;

namespace Pages.Plugin.Areas.Admin
{
    [Area("Admin")]
    public class PageController : BasePluginController
    {
        public override IPlugin Plugin
        {
            get
            {
                return new Pages();
            }
        }

        public PageController(IPluginLog log, ISiteSettingsManagerAsync siteSettingsManager, IPluginSettingsManager pluginSettingsManager,
            IBaseViewModelProvider baseViewModelProvider, IMapper mapper, ISessionService sessionService) :
            base(log, siteSettingsManager, pluginSettingsManager, baseViewModelProvider, mapper, sessionService)
        {

        }

        public IActionResult Index()
        {
            var m = new BaseViewModel();
            return View(m);
        }

        public IActionResult Test()
        {
            var m = new BaseViewModel();
            return View(m);
        }
    }
}
