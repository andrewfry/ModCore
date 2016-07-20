using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ModCore.Core.Controllers;
using ModCore.Abstraction.Site;
using Microsoft.Extensions.Logging;
using ModCore.Abstraction.Plugins;

namespace ModCore.Www.Controllers
{
    public class PluginController : BaseController
    {
        private IPluginManager _pluginManager;

        public PluginController(ILog log, ISessionManager sessionManager, ISiteSettingsManager siteSettingsManager, 
            IPluginManager pluginManager, IBaseViewModelProvider baseModeProvider)
            : base(log,sessionManager,siteSettingsManager, baseModeProvider)
        {
            _pluginManager = pluginManager;
        }

        public IActionResult Activate()
        {
            foreach(var plugin in _pluginManager.AvailablePluginAssemblies)
            {
                _pluginManager.ActivatePlugin(plugin.Item1);
            }

            return Content("Activated");
        }

     
    }
}
