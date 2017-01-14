using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using ModCore.Abstraction.Plugins;
using ModCore.Models.Access;
using ModCore.ViewModels.Access;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModCore.Abstraction.Site
{
    public interface IAuthenticationService
    {

        IPlugin CurrentPlugin();

        Task<User> CreateNewUser(RegisterViewModel registerModel);

        Task<IAuthenticationResult> ValidatePassword(AuthenticationUser authUser, string password);

        Task<IAuthenticationResult> ResetPassword(AuthenticationUser authUser, string password);

        Task<IAuthenticationResult> SignIn(AuthenticationUser authUser, IBaseController controller);

        Task<IAuthenticationResult> SignOut(AuthenticationUser authUser, IBaseController controller);

        Task<bool> UserAllowedAdminAccess(AuthenticationUser authUser, RouteData route);

        IActionResult GetLoginLocation();
    }
}
