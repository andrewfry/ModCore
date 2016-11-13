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

namespace ModCore.Www.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccountController : BaseController
    {
        private IUserService _userService;

        public AccountController(ILog log, ISessionManager sessionManager, ISiteSettingsManager siteSettingsManager,
            IBaseViewModelProvider baseModeProvider, IUserService userService)
            : base(log, sessionManager, siteSettingsManager, baseModeProvider)
        {
            _userService = userService;
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
            result = await _userService.ValidatePassword(user, loginModel.EmailAddress, loginModel.Password);

            if (result)
            {
                this.CurrentSession.UpdateUserData(user, true);
                this.CommitSession();

                return RedirectToLocal(loginModel.ReturnUrl);
            }

            throw new Exception("An unexpected system error has occured.");

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
            DiscardSession();

            return RedirectToAction("Account", "Login", new { Area = "Admin" });
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home", new { Area = "Admin" });
            }
        }

    }
}
