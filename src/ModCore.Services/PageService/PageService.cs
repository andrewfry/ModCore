using ModCore.Services.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ModCore.Models.Page;
using ModCore.Abstraction.Services.PageService;
using ModCore.Abstraction.DataAccess;
using AutoMapper;
using ModCore.Abstraction.Site;
using ModCore.Specifications.Pages;

namespace ModCore.Services.PageService
{
    public class PageService : BaseServiceAsync<Page>, IPageService
    {
        public PageService(IDataRepositoryAsync<Page> repos, IMapper mapper, ILog logger) :
            base(repos, mapper, logger)
        {
        }

        public async Task<Page> GetPageByURL(string requestUrl)
        {
            var page = await _repository.FindAsync(new PageByUrl(requestUrl));

            return page;
        }
    }
}
