using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using ModCore.Abstraction.DataAccess;
using ModCore.Models.Core;

namespace ModCore.Abstraction.Services.Site
{
    public interface ILogService
    {

        Task<IPagedResult<Log>> Filter(List<ISpecification<Log>> queries, IPagedRequest request);
    }
}
