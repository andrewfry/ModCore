using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ModCore.Abstraction.DataAccess;
using ModCore.Core.DataAccess;
using ModCore.ViewModels.Core;
using Pages.Plugin.Models;
using Pages.Plugin.Services;
using Pages.Plugin.Specifications;
using Pages.Plugin.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pages.Plugin.ViewComponents
{
    public class PageListViewComponent : ViewComponent
    {
        private readonly IPageService _pageService;
        private readonly IMapper _mapper;
        public PageListViewComponent(IPageService pageService, IMapper mapper)
        {
            _pageService = pageService;
            _mapper = mapper;
        }

        public async Task<IViewComponentResult> InvokeAsync(int page = 1)
        {
            var pageRequest = new PagedRequest<Page>();
            pageRequest.CurrentPage = page;
            pageRequest.PageSize = 50;
            var specs = new List<ISpecification<Page>>();
            var spec = new GetActivePages(); 
            specs.Add(spec);

            var result = await _pageService.Filter(specs, pageRequest);
            var returnView = _mapper.Map<vPagedResult<PageViewModel>>(result);

            return View(returnView);
        }
    }
}
