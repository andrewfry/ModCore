// Decompiled with JetBrains decompiler
// Type: RoleBasedPermisison.Areas.Admin.Controllers.RolePermissionController
// Assembly: RoleBasedPermission.Plugin, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C83966E3-6571-46EA-A34E-AE63EBCD499A
// Assembly location: C:\Users\Andrew\Desktop\fuck my life\RoleBasedPermission.Plugin.dll

using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ModCore.Abstraction.Plugins;
using ModCore.Abstraction.Services.Access;
using ModCore.Abstraction.Site;
using ModCore.Core.Controllers;
using RoleBasedPermission.Plugin;
using RoleBasedPermission.Plugin.Abstraction;
using RoleBasedPermission.Plugin.ViewModels;
using System.Threading.Tasks;

namespace RoleBasedPermission.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RolePermissionController : BasePluginController
    {
        private IPermissionManagerService _permissionManagerService;

        public override IPlugin Plugin
        {
            get
            {
                return new RoleBasedPermisison();
            }
        }

        public RolePermissionController(IPluginLog log, ISiteSettingsManagerAsync siteSettingsManager, IPluginSettingsManager pluginSettingsManager, IBaseViewModelProvider baseViewModelProvider, IMapper mapper, IPermissionManagerService permissionManagerService, ISessionService sessionService)
          : base(log, siteSettingsManager, pluginSettingsManager, baseViewModelProvider, mapper, sessionService)
        {
            this._permissionManagerService = permissionManagerService;
        }

        public async Task<IActionResult> Index()
        {
            vPermissionManager m = new vPermissionManager();
            return this.View(m);
        }

        public async Task<IActionResult> PermissionEdit(string selectedRoleId)
        {
            vPermissionDiscriptorEdit m = new vPermissionDiscriptorEdit();
            return this.View(m);
        }
    }
}
