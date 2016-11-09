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

namespace ModCore.Www.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccountController : BaseController
    {
        private IUserService _userService;

        public AccountController(ILog log, ISessionManager sessionManager, ISiteSettingsManager siteSettingsManager,
            IBaseViewModelProvider baseModeProvider, IUserService userService)
            : base(log,sessionManager,siteSettingsManager, baseModeProvider)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> Login()
        {
            var m = new LoginViewModel();


            return View(m);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginModel)
        {


            return null;
        }


        [HttpGet]
        public async Task<IActionResult> Register()
        {
            var m = new RegisterViewModel();


            return View(m);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel loginModel)
        {
            if(ModelState.IsValid)
            {
              var user = await  _userService.CreateNewUser(loginModel);
            }


            return View(loginModel);
        }
    }
}
