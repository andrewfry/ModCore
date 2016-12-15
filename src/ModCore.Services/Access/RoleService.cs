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

namespace ModCore.Services.Access
{
    public class RoleService : BaseServiceAsync<Role>, IRoleService
    {

        public RoleService(IDataRepositoryAsync<Role> repos, IMapper mapper, ILog logger,
            ISiteSettingsManagerAsync siteSettings) :
            base(repos, mapper, logger)
        {
        }
              
        public async Task<List<Role>> GetAllRolesAsync()
        {
            var result = await _repository.FindAllAsync(new GetAllRoles());

            return result.ToList();
        }

        
    }
}
