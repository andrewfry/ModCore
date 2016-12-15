using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ModCore.Abstraction.Plugins;
using ModCore.Abstraction.Services.Access;
using ModCore.Abstraction.Site;
using ModCore.Core.Controllers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Plugin.Controllers
{
    public class PluginController : BasePluginController
    {

        public override IPlugin Plugin
        {
            get
            {
                return new Blog();
            }
        }

        public PluginController(IPluginLog log, ISiteSettingsManagerAsync siteSettingsManager, IPluginSettingsManager pluginSettingsManager,
            IBaseViewModelProvider baseViewModelProvider, IMapper _mapper, ISessionService sessionService) :
            base(log, siteSettingsManager, pluginSettingsManager, baseViewModelProvider, _mapper, sessionService)
        {

        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Test()
        {
            var obj = JsonConverter.Equals(new { }, new { });

            return Content("hi");
        }

    }
}
