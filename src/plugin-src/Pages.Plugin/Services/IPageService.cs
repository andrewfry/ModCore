using ModCore.Abstraction.DataAccess;
using ModCore.Abstraction.Services;
using ModHtml.Dependency;
using Pages.Plugin.Models;
using Pages.Plugin.ViewModels;
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

        Task<Page> Update(PageViewModel model);

        Task Delete(string Id);

        Task Delete(Page page);

        Task<IPagedResult<Page>> Filter(List<ISpecification<Page>> queries, IPagedRequest request);

        IList<IHtmlComponent> AvailableHTMLComponents();

    }
}
