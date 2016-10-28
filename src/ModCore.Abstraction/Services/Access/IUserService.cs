using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using ModCore.Models.Access;
using System.Security.Claims;
using ModeCore.ViewModels.Access;

namespace ModCore.Abstraction.Services.Access
{
    public interface IUserService : IService<User>
    {
        bool ValidateLastChanged(ClaimsPrincipal userPrincipal, DateTime lastChanged);

        Task ValidatePassword(User user, string password);

        Task ResetPassword(User user, string password);

        Task<User> CreateNewUser(RegisterViewModel registerModel);

        Task<User> GetUser(string userId);
    }
}
