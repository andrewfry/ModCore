using AutoMapper;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.Extensions.Logging;
using ModCore.Abstraction.Plugins;
using ModCore.Abstraction.Site;
using ModCore.Core.Constraints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModCore.Core.Controllers
{
    public abstract class BasePluginController : BaseController
    {
        private IPluginSettingsManager _pluginSettingsManager;

        public IPluginSettingsManager PluginSettingsManager
        {
            get
            {
                return _pluginSettingsManager;
            }
        }

        public abstract IPlugin Plugin { get; }


        public BasePluginController(IPluginLog log, ISessionManager sessionManager, ISiteSettingsManager siteSettingsManager, IPluginSettingsManager pluginSettingsManager,
            IBaseViewModelProvider baseModelProvider, IMapper mapper) :
            base(log, sessionManager, siteSettingsManager, baseModelProvider, mapper)
        {
            log.SetPlugin(this.Plugin);
            _pluginSettingsManager = pluginSettingsManager;

        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            context.HttpContext.Items.Add("PluginName", this.Plugin.Name);

            base.OnActionExecuting(context);
        }

    }
}
