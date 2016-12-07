using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using ModCore.Models.Access;
using System.Security.Claims;
using ModCore.ViewModels.Access;
using Microsoft.AspNetCore.Routing;
using ModCore.Abstraction.DataAccess;

namespace ModCore.Abstraction.Services.Access
{
    public interface IUserActivityService : IServiceAsync<UserActivity>
    {
        Task AddActivity(UserActivity userActivity);

        Task<IPagedResult<UserActivity>> Filter(List<ISpecification<UserActivity>> queries, IPagedRequest request);
    }
}
