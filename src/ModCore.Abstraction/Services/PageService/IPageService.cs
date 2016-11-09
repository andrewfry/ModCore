using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ModCore.Models.Page;

namespace ModCore.Abstraction.Services.PageService
{
    public interface IPageService : IServiceAsync<Page>
    {
        Task<Page> GetPageByURL(string requestUrl);
    }
}
