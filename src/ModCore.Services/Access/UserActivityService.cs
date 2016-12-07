using AutoMapper;
using ModCore.Abstraction.Services.Access;
using ModCore.Abstraction.Site;
using ModCore.Models.Access;
using ModCore.Services.Base;
using ModCore.Abstraction.DataAccess;
using ModCore.Models.Core;
using ModCore.Abstraction.Services.Site;
using System;
using System.Collections.Generic;
using ModCore.Specifications.BuiltIns;
using System.Threading.Tasks;
using ModCore.Core.DataAccess;

namespace ModCore.Services.Access
{
    public class UserActivityService : BaseServiceAsync<UserActivity>, IUserActivityService
    {
        private readonly ISiteSettingsManagerAsync _siteSettings;

        public UserActivityService(IDataRepositoryAsync<UserActivity> repos, IMapper mapper, ILog logger,
            ISiteSettingsManagerAsync siteSettings) :
            base(repos, mapper, logger)
        {
            _siteSettings = siteSettings;
        }

        public async Task AddActivity(UserActivity userActivity)
        {
            userActivity.InsertDate = DateTime.UtcNow;

            await _repository.InsertAsync(userActivity);
        }

        public async Task<IPagedResult<UserActivity>> Filter(List<ISpecification<UserActivity>> queries, IPagedRequest request)
        {
            ISpecification<UserActivity> finalSpecification;

            if (queries.Count == 0)
            {
                throw new ArgumentException($"{nameof(queries)} must have at least one specification");
            }

            finalSpecification = queries[0];

            if (queries.Count > 1)
            {
                for (int i = 1; i < queries.Count; i++)
                {
                    finalSpecification = finalSpecification.And<UserActivity>(queries[i]);
                }
            }

            var result = await _repository.FindAllByPageAsync(finalSpecification, request);

            return result;
        }

    }
}
