using ModCore.Abstraction.Services;
using Pages.Plugin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pages.Plugin.Services
{
    public interface IPageService : IServiceAsync<Page>
    {
        Task<Page> GetPageByURL(string requestUrl);

        Task<Page> CreatePage(PageViewModel newPage);

        Task<ICollection<PageViewModel>> PageList();
    }
}
