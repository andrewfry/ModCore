using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ModCore.Core.Controllers;
using ModCore.Abstraction.Site;
using Microsoft.Extensions.Logging;
using ModCore.ViewModels.Base;
using ModCore.ViewModels.Access;
using ModCore.Abstraction.Services.Access;
using System.Security.Claims;
using ModCore.Models.Access;
using AutoMapper;

namespace ModCore.Www.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccountController : BaseController
    {
        private IUserService _userService;
        private IAuthenticationService _authService;

        public AccountController(ILog log,  ISiteSettingsManagerAsync siteSettingsManager,
            IBaseViewModelProvider baseModeProvider, IMapper mapper, IUserService userService, IAuthenticationService authService)
            : base(log,  siteSettingsManager, baseModeProvider, mapper)
        {
            _userService = userService;
            _authService = authService;
        }

        [HttpGet]
        public async Task<IActionResult> Login(string ReturnUrl)
        {
            var m = new LoginViewModel();
            m.ReturnUrl = ReturnUrl;

            return View(m);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel loginModel)
        {
            var result = false;
            User user = null;

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Summary", "The password or user name did not match");

                return View(loginModel);
            }

            user = await _userService.GetByEmail(loginModel.EmailAddress);
            var authuser = _mapper.Map<AuthenticationUser>(user);
            var validationResult = await _authService.ValidatePassword(authuser, loginModel.Password);
            result = validationResult.Successful;

            if (result)
            {
                var authResult = await _authService.SignIn(authuser, this);

                if (!string.IsNullOrEmpty(loginModel.ReturnUrl))
                {
                    if (Url.IsLocalUrl(loginModel.ReturnUrl))
                    {
                        return Redirect(loginModel.ReturnUrl);
                    }
                }

                if (authResult.HasResult)
                {
                    return authResult.ActionResult;
                }
                else
                {
                    throw new Exception($"{_authService.CurrentPlugin().Name}'s AuthenticatioService does not have a return ActionResult.");
                }
            }

            ModelState.AddModelError("Summary", validationResult.ErrorMessage);
            return View(loginModel);
        }

        [HttpGet]
        public async Task<IActionResult> Register()
        {
            var m = new RegisterViewModel();


            return View(m);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel loginModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userService.CreateNewUser(loginModel);
            }


            return View(loginModel);
        }

        [HttpGet]
        public async Task<IActionResult> LogOut()
        {
            var authUser = _mapper.Map<AuthenticationUser>(this.CurrentSession.UserData);
            var authResult = await _authService.SignOut(authUser, this);

            if (authResult.HasResult)
            {
                return authResult.ActionResult;
            }
            else
            {
                return RedirectToAction("Account", "Login", new { Area = "Admin" });
            }
        }
    }
}
