using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.DependencyInjection;
using ModCore.Abstraction.Services.Access;

namespace ModCore.Core.Middleware
{
    public static class LastChangedValidator
    {
        public static async Task ValidateAsync(CookieValidatePrincipalContext context)
        {
            // Pull database from registered DI services.
            var userService = context.HttpContext.RequestServices.GetRequiredService<IUserService>();
            var userPrincipal = context.Principal;

            // Look for the last changed claim.
            string lastChanged;
            lastChanged = (from c in userPrincipal.Claims
                           where c.Type == "LastUpdated"
                           select c.Value).FirstOrDefault();

            DateTime lastChangedDate = DateTime.Parse(lastChanged);
            var lastChange = await userService.ValidateLastChanged(userPrincipal, lastChangedDate);

            if (string.IsNullOrEmpty(lastChanged) || !lastChange)
            {
                context.RejectPrincipal();
                await context.HttpContext.Authentication.SignOutAsync("ModCoreBasicCookieAuth");
            }
        }
    }
}
