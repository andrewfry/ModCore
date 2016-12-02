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
using ModCore.Core.Controllers;
using ModCore.Models.Page;
using ModCore.Abstraction.Plugins;
using ModCore.Abstraction.Services.PageService;
using ModCore.ViewModels.Page;

namespace Pages.Plugin.Services
{
    public class PageService : BaseService
    {
        private readonly IPageService _pageService;

        public PageService(IPageService pageService, IMapper mapper, ILog logger) : base(mapper, logger)
        {
            _pageService = pageService;
        }

        public IPlugin CurrentPlugin()
        {
            return new Pages();
        }

        public async Task<Page> GetPageByURL(string url)
        {
            return await _pageService.GetPageByURL(url);
        }
        public async Task<Page> CreatePage(PageViewModel newPage)
        {
            return await _pageService.CreatePage(newPage);
        }


    }
}
