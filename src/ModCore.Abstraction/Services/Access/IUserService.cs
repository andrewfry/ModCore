using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using ModCore.Models.Access;
using System.Security.Claims;
using ModCore.ViewModels.Access;

namespace ModCore.Abstraction.Services.Access
{
    public interface IUserService : IServiceAsync<User>
    {
        Task<bool> ValidateLastChanged(ClaimsPrincipal userPrincipal, DateTime lastChanged);

        Task<bool> ValidatePassword(string userId, string password);

        Task ResetPassword(string userId, string password);

        Task<User> CreateNewUser(RegisterViewModel registerModel);
    }
}
