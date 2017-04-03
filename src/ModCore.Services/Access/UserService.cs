using AutoMapper;
using Microsoft.Extensions.Options;
using ModCore.Abstraction.Services.Access;
using ModCore.Abstraction.Site;
using ModCore.Models.Access;
using ModCore.Services.Base;
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
using ModCore.Services.Exceptions;
using Microsoft.AspNetCore.Routing;
using ModCore.Core.Site;
using ModCore.Models.Core;

namespace ModCore.Services.Access
{
    public class UserService : BaseServiceAsync<User>, IUserService
    {
        private readonly ISiteSettingsManagerAsync _siteSettings;

        public UserService(IDataRepositoryAsync<User> repos, IMapper mapper, ILog logger,
            ISiteSettingsManagerAsync siteSettings) :
            base(repos, mapper, logger)
        {
            _siteSettings = siteSettings;
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

        public async Task<ResultPacket<User>> CreateNewUser(vRegister model)
        {
            try
            {
                var existingUser = await _repository.FindAsync(new GetByEmail(model.EmailAddress));
                if (existingUser != null)
                    throw new DuplicateUserException($"A user with the email: {model.EmailAddress} already exists.");

                var user = _mapper.Map<User>(model);
                user.PasswordSalt = SecurityUtil.GetSalt();
                user.PasswordHash = SecurityUtil.GetHash(model.Password + user.PasswordSalt);
                user.FailedLoginAttempts = 0;
                user.DateCreated = DateTime.UtcNow;
                user.EmailHashVerification = SecurityUtil.GetRandomHash(16);
                user.EmailVerified = false;

                await _repository.InsertAsync(user);

                return new SuccessPacket<User>(user);
            }
            catch (DuplicateUserException ex)
            {
                return new FailurePacket<User>(ex, ex.Message);
            }
            catch (Exception ex)
            {
                return new FailurePacket<User>(ex);
            }
            
        }

        public async Task IncrementFailedLogin(User user)
        {
            user.FailedLoginAttempts += 1;

            var lockOutNumber = await _siteSettings.GetSettingAsync<int>(BuiltInSettings.AuthenticationLockOut);
            if (lockOutNumber > user.FailedLoginAttempts)
                user.LockedOut = true;

            await _repository.UpdateAsync(user);
        }

        public async Task<User> GetByEmailAsync(string emailAddress)
        {
            return await _repository.FindAsync(new GetByEmail(emailAddress));
        }

        
    }
}
