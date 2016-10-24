using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using ModCore.Models.Access;
using System.Security.Claims;

namespace ModCore.Abstraction.Services.Access
{
    public interface IUserService : IService<User>
    {
        bool ValidateLastChanged(ClaimsPrincipal userPrincipal, DateTime lastChanged);
    }
}
