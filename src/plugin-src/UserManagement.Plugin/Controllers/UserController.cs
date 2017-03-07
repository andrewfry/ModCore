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
using ModCore.ViewModels.Theme;
using ModCore.Abstraction.Services.Access;
using ModCore.ViewModels.Access;

namespace UserManagement.Plugin.Controllers
{
    public class UserController : BasePluginController
    {
        public override IPlugin Plugin
        {
            get
            {
                return new UserManagement.Plugin.UserManagementPlugin();
            }
        }

        private IUserService _userService;

        public UserController(IPluginLog log,ISiteSettingsManagerAsync siteSettingsManager, IPluginSettingsManager pluginSettingsManager,
            IBaseViewModelProvider baseViewModelProvider, IMapper mapper, ISessionService sessionService, IUserService userService) :
            base(log, siteSettingsManager, pluginSettingsManager, baseViewModelProvider, mapper, sessionService)
        {
            _userService = userService;
        }


        [HttpGet]
        public IActionResult Register()
        {

            return View();
        }

        [HttpPost]
        public IActionResult Register(vRegister model)
        {

            return View();
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {

            return View();
        }

        [HttpPost]
        public IActionResult ForgotPassword(vForgotPassword model)
        {

            return View();
        }

        [HttpGet]
        public IActionResult ChangePassword()
        {

            return View();
        }

        [HttpPost]
        public IActionResult ChangePassword(vResetPassword model)
        {

            return View();
        }

    }
}
