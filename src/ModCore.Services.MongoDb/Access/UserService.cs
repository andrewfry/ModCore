using ModCore.Abstraction.Services.Access;
using ModCore.Models.Access;
using ModeCore.ViewModels.Access;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ModCore.Services.MongoDb.Access
{
    public class UserService : IUserService
    {

        public UserService()
        {

        }

        public bool ValidateLastChanged(ClaimsPrincipal userPrincipal, DateTime lastChanged)
        {
            throw new NotImplementedException();
        }

        public Task ValidatePassword(User user, string password)
        {
            throw new NotImplementedException();
        }

        public Task ResetPassword(User user, string password)
        {
            throw new NotImplementedException();
        }

        public Task<User> CreateNewUser(RegisterViewModel registerModel)
        {
            throw new NotImplementedException();
        }

        public Task<User> GetUser(string userId)
        {
            throw new NotImplementedException();
        }

    }
}
