using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ModCore.Abstraction.Plugins;
using ModCore.Abstraction.Services.Access;
using ModCore.Abstraction.Site;
using ModCore.Core.Controllers;
using ModCore.ViewModels.Access;
using RoleBasedPermission.Plugin;
using RoleBasedPermission.Plugin.Abstraction;
using RoleBasedPermission.Plugin.ViewModels;
using System.Linq;
using System.Threading.Tasks;

namespace RoleBasedPermission.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RolePermissionController : BasePluginController
    {
        private IPermissionManagerService _permissionManagerService;
        private IRoleService _roleService;

        public override IPlugin Plugin
        {
            get
            {
                return new RoleBasedPermission.Plugin.RoleBasedPermission();
            }
        }

        public RolePermissionController(IPluginLog log, ISiteSettingsManagerAsync siteSettingsManager, IRoleService roleService,  IPluginSettingsManager pluginSettingsManager, IBaseViewModelProvider baseViewModelProvider, IMapper mapper, IPermissionManagerService permissionManagerService, ISessionService sessionService)
          : base(log, siteSettingsManager, pluginSettingsManager, baseViewModelProvider, mapper, sessionService)
        {
            _permissionManagerService = permissionManagerService;
            _roleService = roleService;
        }

        public async Task<IActionResult> Index(string selectedRoleId)
        {
            vPermissionManager m = new vPermissionManager();
            var roles =  await _roleService.GetAllRolesAsync();
            m.Roles = roles.Select(a => _mapper.Map<vRole>(a)).ToList();
            m.SelectedRoleId = selectedRoleId;

            return this.View(m);
        }

        public async Task<IActionResult> PermissionEdit(string selectedRoleId)
        {
            vPermissionDiscriptorEdit m = new vPermissionDiscriptorEdit();
            return this.View(m);
        }

        public async Task<IActionResult> PermissionSave(string selectedRoleId)
        {
            vPermissionDiscriptorEdit m = new vPermissionDiscriptorEdit();
            return this.View(m);
        }
    }
}
