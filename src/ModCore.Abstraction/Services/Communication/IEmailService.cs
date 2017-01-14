using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using ModCore.Models.Access;
using System.Security.Claims;
using ModCore.ViewModels.Access;
using Microsoft.AspNetCore.Routing;
using ModCore.Models.Communication;

namespace ModCore.Abstraction.Services.Access
{
    public interface IEmailService : IServiceAsync<EmailTemplate>
    {
        
    }
}
