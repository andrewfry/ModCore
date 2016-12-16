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
using RoleBasedPermisison.Abstraction;
using RoleBasedPermisison.ViewModels;

namespace RoleBasedPermisison.Controllers
{
    public class RolePermissionController : BasePluginController
    {
        public override IPlugin Plugin
        {
            get
            {
                return new RoleBasedPermisison.Plugin.RoleBasedPermisison();
            }
        }

        private IPermissionManagerService _permissionManagerService;

        public RolePermissionController(IPluginLog log,ISiteSettingsManagerAsync siteSettingsManager, IPluginSettingsManager pluginSettingsManager,
            IBaseViewModelProvider baseViewModelProvider, IMapper mapper, IPermissionManagerService permissionManagerService, ISessionService sessionService) :
            base(log, siteSettingsManager, pluginSettingsManager, baseViewModelProvider, mapper, sessionService)
        {
            _permissionManagerService = permissionManagerService;
        }

        public async Task<IActionResult> Index()
        {
            var m = new vPermissionManager();
         
            return View(m);
        }

        public async Task<IActionResult> PermissionEdit(string selectedRoleId)
        {
            var m = new vPermissionDiscriptorEdit();

            return View(m);
        }


    }
}
