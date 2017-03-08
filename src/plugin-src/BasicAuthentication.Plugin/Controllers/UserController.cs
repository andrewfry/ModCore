using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ModCore.Abstraction.Plugins;
using ModCore.Abstraction.Services.Access;
using ModCore.Abstraction.Site;
using ModCore.Core.Controllers;
using ModCore.ViewModels.Access;
using ModCore.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace BasicAuthentication.Plugin.Controllers
{
    public class UserManagementController : BasePluginController
    {

        public override IPlugin Plugin
        {
            get
            {
                return new BasicAuthentication();
            }
        }

        private bool _allowedToRegister;
        private bool _requireEmailValidation;
        private IAuthenticationService _authService;

        public UserManagementController(IAuthenticationService authService, IPluginLog log, ISiteSettingsManagerAsync siteSettingsManager, IPluginSettingsManager pluginSettingsManager, IBaseViewModelProvider baseViewModelProvider, IMapper mapper, ISessionService sessionService)
          : base(log, siteSettingsManager, pluginSettingsManager, baseViewModelProvider, mapper, sessionService)
        {
            _allowedToRegister = PluginSettingsManager.GetSettingAsync<bool>(BasicAuthentication.BuiltInSettings.RegisterUserEnabled).Result;
            _requireEmailValidation = PluginSettingsManager.GetSettingAsync<bool>(BasicAuthentication.BuiltInSettings.RegisterUserEnabled).Result;

            _authService = authService;
        }


        [HttpGet]
        public async Task<IActionResult> Register()
        {
            if (!_allowedToRegister)
                return ReturnError(HttpStatusCode.NotFound);

            var bm = new BaseViewModel();

            return View(bm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(vRegister model)
        {
            if (!_allowedToRegister)
                return ReturnError(HttpStatusCode.NotFound);

            if (ModelState.IsValid)
            {
                var result = await _authService.CreateNewUser(model);

                if (result.Successful)
                {
                    return View("RegisterSuccessful", new BaseViewModel());
                }
            }

            var vm = new BaseViewModel();

            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> ForgotPassword()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(vForgotPassword model)
        {

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ChangePassword()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(vResetPassword model)
        {

            return View();
        }


    }
}
