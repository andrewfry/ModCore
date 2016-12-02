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
using ModCore.ViewModels.Page;

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

        public async Task<Page> CreatePage(PageViewModel newPage)
        {
            var page = _mapper.Map<Page>(newPage);

            var existingPage = await _repository.FindAsync(new PageByUrl(page.FriendlyURL));
            if (existingPage != null)
                throw new Exception($"A page with url: {existingPage.FriendlyURL} already exists.");

            await _repository.InsertAsync(page);

            return page;
        }
    }
}
