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
using UserManagement.Plugin;

namespace UserManagement.Plugin.Admin.Controllers
{
    [Area("Admin")]
    public class UserManagementController : BasePluginController
    {

        public override IPlugin Plugin
        {
            get
            {
                return new UserManagement.Plugin.UserManagementPlugin();
            }
        }

        public UserManagementController(IPluginLog log, ISiteSettingsManagerAsync siteSettingsManager, IPluginSettingsManager pluginSettingsManager, IBaseViewModelProvider baseViewModelProvider, IMapper mapper, ISessionService sessionService)
          : base(log, siteSettingsManager, pluginSettingsManager, baseViewModelProvider, mapper, sessionService)
        {

        }

        public async Task<IActionResult> Index()
        {

            return this.View(null);
        }

    }
}
