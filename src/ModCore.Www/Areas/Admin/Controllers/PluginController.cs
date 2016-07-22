using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ModCore.Core.Controllers;
using ModCore.Abstraction.Site;
using Microsoft.Extensions.Logging;
using ModCore.ViewModels.Base;
using ModCore.Abstraction.Plugins;
using ModCore.ViewModels.Admin.Plugin;
using ModCore.Core.Plugins;

namespace ModCore.Www.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PluginController : BaseController
    {
        private IPluginManager _pluginManager;

        public PluginController(ILog log, ISessionManager sessionManager, ISiteSettingsManager siteSettingsManager,
            IBaseViewModelProvider baseModeProvider, IPluginManager pluginManager)
            : base(log,sessionManager,siteSettingsManager, baseModeProvider)
        {
            _pluginManager = pluginManager;
        }

        public IActionResult Index()
        {
            var m = new vPluginList();
            m.Plugins = _pluginManager.AvailablePlugins.Select(a => new vPlugin
            {
                PluginName = a.Name,
                PluginDescription = a.Description,
                PluginVersion = a.Version
            }).ToList();

            return View(m);
        }

    }
}
