using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using ModCore.Models.Access;
using System.Security.Claims;
using ModCore.ViewModels.Access;
using Microsoft.AspNetCore.Routing;

namespace ModCore.Abstraction.Services.Access
{
    public interface IUserService : IServiceAsync<User>
    {
        Task<bool> ValidateLastChanged(ClaimsPrincipal userPrincipal, DateTime lastChanged);

        Task<bool> ValidatePassword(User user, string emailAddress, string password);

        Task<bool> ValidatePassword(string emailAddress, string password);

        Task ResetPassword(string emailAddress, string password);

        Task<User> CreateNewUser(RegisterViewModel registerModel);

        Task<User> GetByEmail(string emailAddress);

        Task<bool> UserAllowedAdminAccess(string userId, RouteData route);
    }
}
