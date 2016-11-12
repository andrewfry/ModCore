using AutoMapper;
using Microsoft.Extensions.Options;
using ModCore.Abstraction.Services.Access;
using ModCore.Abstraction.Site;
using ModCore.Models.Access;
using ModCore.Services.MongoDb.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;
using ModCore.Utilities.Security;
using ModCore.Abstraction.DataAccess;
using ModCore.ViewModels.Access;
using ModCore.Specifications.Access;
using ModCore.Services.MongoDb.Exceptions;
using Microsoft.AspNetCore.Routing;

namespace ModCore.Services.MongoDb.Access
{
    public class UserService : BaseServiceAsync<User>, IUserService
    {

        public UserService(IDataRepositoryAsync<User> repos, IMapper mapper, ILog logger) :
            base(repos, mapper, logger)
        {
        }

        public async Task<bool> ValidateLastChanged(ClaimsPrincipal userPrincipal, DateTime lastChanged)
        {
            var userId = userPrincipal.Identity.Name;
            var user = await this.GetByIdAsync(userId);

            if (lastChanged < user.LastUpdateDate)
            {
                return true;
            }

            return false;
        }

        public async Task<bool> ValidatePassword(string emailAddress, string password)
        {
            var user = await _repository.FindAsync(new GetByEmail(emailAddress));

            return await ValidatePassword(user, emailAddress, password);
        }

        public async Task<bool> ValidatePassword(User user, string emailAddress, string password)
        {
            if (user.LockedOut)
                return false;

            var sentHash = SecurityUtil.GetHash(password + user.PasswordSalt);
            var result = string.Compare(user.PasswordHash, sentHash) == 0;

            if (!result)
            {
                await IncrementFailedLogin(user);
            }

            return result;
        }

        public async Task ResetPassword(string userId, string password)
        {
            var user = await GetByIdAsync(userId);

            user.PasswordSalt = SecurityUtil.GetSalt();
            user.PasswordHash = SecurityUtil.GetHash(password + user.PasswordSalt);
            user.FailedLoginAttempts = 0;
            user.LastPasswordReset = DateTime.UtcNow;

            await _repository.UpdateAsync(user);

            //Send Password Reset Email
        }

        public async Task<User> CreateNewUser(RegisterViewModel registerModel)
        {
            var user = _mapper.Map<User>(registerModel);

            var existingUser = await _repository.FindAsync(new GetByEmail(user.EmailAddress));
            if (existingUser != null)
                throw new DuplicateUserException($"A user with the email: {user.EmailAddress} already exists.");

            user.PasswordSalt = SecurityUtil.GetSalt();
            user.PasswordHash = SecurityUtil.GetHash(registerModel.Password + user.PasswordSalt);
            user.FailedLoginAttempts = 0;
            user.DateCreated = DateTime.UtcNow;

            var randomBytes = SecurityUtil.GetRandomBytes(16);
            var guid = Guid.NewGuid().ToString();
            var emailHash = SecurityUtil.GetHash(randomBytes + guid);

            user.EmailHashVerification = emailHash;
            user.EmailVerified = false;

            await _repository.InsertAsync(user);

            return user;
        }

        //TODO - we need to bring in the site settings and then lockt he account out if it exceeds X
        public async Task IncrementFailedLogin(User user)
        {
            user.FailedLoginAttempts += 1;

            await _repository.UpdateAsync(user);
        }

        public async Task<User> GetByEmail(string emailAddress)
        {
            return await _repository.FindAsync(new GetByEmail(emailAddress));
        }

        public async Task<bool> UserAllowedAdminAccess(string userId, RouteData route)
        {

            return  true;
        }
    }
}
