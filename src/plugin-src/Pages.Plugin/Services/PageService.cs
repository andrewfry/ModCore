using AutoMapper;
using Microsoft.Extensions.Options;
using ModCore.Abstraction.Services.Access;
using ModCore.Abstraction.Site;
using ModCore.Models.Access;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Routing;
using ModCore.ViewModels.Access;
using ModCore.Services.Base;
using ModCore.Specifications.Base;
using ModCore.Core.Controllers;
using Pages.Plugin.Models;
using Pages.Plugin.ViewModels;
using ModCore.Abstraction.DataAccess;
using Pages.Plugin.Specifications;
using Pages.Plugin.Mapping;

namespace Pages.Plugin.Services
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

            var existingPage = await _repository.FindAsync(new PageByUrl(page.Url));
            if (existingPage != null)
                throw new Exception($"A page with url: {existingPage.Url} already exists.");

            await _repository.InsertAsync(page);

            return page;
        }

        public async Task<ICollection<PageViewModel>> PageList()
        {
            var pageList = await _repository.FindAllAsync();
            var result = _mapper.Map<ICollection<PageViewModel>>(pageList);
            return result;
        }

        public async Task<Page> Update(PageViewModel model)
        {
            var result = await _repository.UpdateAsync(_mapper.Map<Page>(model));
            return _mapper.Map<Page>(result);
        }

        public async Task Delete(string Id)
        {
            await _repository.DeleteAsync(new GetById<Page>(Id));
        }

        public async Task Delete(Page page)
        {
            await _repository.DeleteAsync(new GetById<Page>(page.Id));
        }
    }
}
