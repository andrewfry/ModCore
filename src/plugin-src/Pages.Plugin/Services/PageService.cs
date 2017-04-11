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
using ModCore.Specifications.BuiltIns;
using ModHtml.Dependency;
using System.Reflection;

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

        public async Task<IPagedResult<Page>> Filter(List<ISpecification<Page>> queries, IPagedRequest request)
        {
            ISpecification<Page> finalSpecification;

            if (queries.Count == 0)
            {
                throw new ArgumentException($"{nameof(queries)} must have at least one specification");
            }

            finalSpecification = queries[0];

            if (queries.Count > 1)
            {
                for (int i = 1; i < queries.Count; i++)
                {
                    finalSpecification = finalSpecification.And<Page>(queries[i]);
                }
            }

            var result = await _repository.FindAllByPageAsync(finalSpecification, request);

            return result;
        }

        public IList<IHtmlComponent> AvailableHTMLComponents()
        {
            List<IHtmlComponent> _availableComponents = new List<IHtmlComponent>();
            var navMenu = new ModHtml.Dependency.HtmlComponentTypes.NavigationMenu();
            navMenu.DisplayTypeName = "Navigation Menu";
            _availableComponents.Add(navMenu);

            var textArea = new ModHtml.Dependency.HtmlComponentTypes.TextArea();
            textArea.DisplayTypeName = "Text Area";
            _availableComponents.Add(textArea);
            return _availableComponents;
        }

    }
}
