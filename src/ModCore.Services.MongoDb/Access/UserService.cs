using AutoMapper;
using Microsoft.Extensions.Options;
using ModCore.Abstraction.Services.Access;
using ModCore.Abstraction.Site;
using ModCore.Models.Access;
using ModCore.Services.MongoDb.Base;
using ModeCore.ViewModels.Access;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;
using ModCore.Utilities.Security;

namespace ModCore.Services.MongoDb.Access
{
    public class UserService : BaseServiceAsync<User>, IUserService
    {

        public UserService(IOptions<MongoDbSettings> dbSettings, IMapper mapper, ILog logger) :
            base(dbSettings, mapper, logger)
        {
        }

        public bool ValidateLastChanged(ClaimsPrincipal userPrincipal, DateTime lastChanged)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ValidatePassword(string userId, string password)
        {
            var user = await this.GetByIdAsync(userId);
            var sentHash = SecurityUtil.GetHash(password + user.PasswordSalt);

            return string.Compare(user.PasswordHash, sentHash) == 0;
        }

        public async Task ResetPassword(string userId, string password)
        {
            throw new NotImplementedException();
        }

        public async Task<User> CreateNewUser(RegisterViewModel registerModel)
        {
            var user = Mapper.Map<User>(registerModel);

            user.PasswordSalt = SecurityUtil.GetSalt();
            user.PasswordHash = SecurityUtil.GetHash(registerModel.Password + user.PasswordSalt);
            user.FailedLoginAttempts = 0;
            user.DateCreated = DateTime.UtcNow;

            await _repository.InsertAsync(user);

            return user;
        }

    }
}
