using AutoMapper;
using Microsoft.Extensions.Options;
using ModCore.Abstraction.Services.Access;
using ModCore.Abstraction.Site;
using ModCore.Models.Access;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ModCore.Utilities.Security;
using BasicAuthentication.Plugin.Models;
using Microsoft.AspNetCore.Routing;
using ModCore.ViewModels.Access;
using ModCore.Services.Base;
using ModCore.Core.Controllers;
using ModCore.Models.Sessions;
using ModCore.Abstraction.Plugins;

namespace BasicAuthentication.Plugin.Services
{
    public class AuthenticationService : BaseService, IAuthenticationService
    {
        private readonly ISiteSettingsManagerAsync _siteSettings;
        private readonly IUserService _userService;

        public AuthenticationService(IUserService userService, IMapper mapper, ILog logger,
            ISiteSettingsManagerAsync siteSettings) :
            base(mapper, logger)
        {
            _userService = userService;
            _siteSettings = siteSettings;
        }

        public IPlugin CurrentPlugin()
        {
            return new BasicAuthentication();
        }

        public async Task<User> CreateNewUser(RegisterViewModel registerModel)
        {
            return await _userService.CreateNewUser(registerModel);
        }

        public async Task<IAuthenticationResult> ValidatePassword(AuthenticationUser authUser, string password)
        {
            var user = await _userService.GetByIdAsync(authUser.Id);

            if (user.LockedOut)
                return new BasicAuthenticationResult(false, "The user is currently locked out.");

            var sentHash = SecurityUtil.GetHash(password + user.PasswordSalt);
            var result = string.Compare(user.PasswordHash, sentHash) == 0;

            if (!result)
            {
                await _userService.IncrementFailedLogin(user);
                return new BasicAuthenticationResult(false, "The information does not match our records");
            }

            return new BasicAuthenticationResult(true);
        }

        public async Task<IAuthenticationResult> ResetPassword(AuthenticationUser authUser, string password)
        {
            try
            {
                await _userService.ResetPassword(authUser.Id, password);

                return new BasicAuthenticationResult(true);
            }
            catch (Exception ex)
            {
                _logger.LogError<AuthenticationService>(ex, "User Id {0}, failed to reset password", authUser.Id);
                return new BasicAuthenticationResult(false, "An error occurred");
            }
        }

        public async Task<IAuthenticationResult> SignIn(AuthenticationUser authUser, IBaseController controller)
        {
            try
            {
                var result = new BasicAuthenticationResult(true);
                var baseController =  controller as BaseController;

                if(baseController == null)
                {
                    throw new InvalidCastException("The IBaseController is NOT a base controller");
                }

                baseController.CurrentSession.IsLoggedIn = true;
                baseController.CurrentSession.UserId = authUser.Id;
                baseController.CurrentSession.UserData = _mapper.Map<SessionUserData>(authUser);
                baseController.CommitSession();

                result.SetResult(baseController.RedirectToAction("Account", "Login", new { Area = "Admin" }));

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError<AuthenticationService>(ex, "User Id {0}, failed to SignIn", authUser.Id);
                return new BasicAuthenticationResult(false, "An error occurred");
            }
        }

        public async Task<IAuthenticationResult> SignOut(AuthenticationUser authUser, IBaseController controller)
        {
            try
            {
                var result = new BasicAuthenticationResult(true);
                var baseController = controller as BaseController;

                if (baseController == null)
                {
                    throw new InvalidCastException("The IBaseController is NOT a base controller");
                }

                baseController.CurrentSession.IsLoggedIn = false;
                baseController.CurrentSession.UserId = string.Empty;
                baseController.CurrentSession.UserData = null;
                baseController.CommitSession();

                result.SetResult(baseController.RedirectToAction("Account", "Login", new { Area = "Admin" }));

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError<AuthenticationService>(ex, "User Id {0}, failed to SignIn", authUser.Id);
                return new BasicAuthenticationResult(false, "An error occurred");
            }
        }

        public async Task<bool> UserAllowedAdminAccess(AuthenticationUser authUser, RouteData route)
        {

            return true;
        }
    }
}
