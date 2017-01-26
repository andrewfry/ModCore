using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModCore.Abstraction.Plugins;
using ModCore.Abstraction.Services.Access;
using ModCore.Abstraction.Site;
using ModCore.Core.Controllers;
using ModCore.ViewModels.Access;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Emailer.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class EmailerController : BasePluginController
    {

        public override IPlugin Plugin
        {
            get
            {
                return new Emailer.Plugin.Emailer();
            }
        }

        public EmailerController(IPluginLog log, ISiteSettingsManagerAsync siteSettingsManager, IPluginSettingsManager pluginSettingsManager, IBaseViewModelProvider baseViewModelProvider, IMapper mapper, ISessionService sessionService)
          : base(log, siteSettingsManager, pluginSettingsManager, baseViewModelProvider, mapper, sessionService)
        {

        }

        public async Task<IActionResult> Index(string selectedRoleId)
        {

            return this.View(null);
        }

    }
}
