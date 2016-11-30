using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using ModCore.Abstraction.DataAccess;
using ModCore.Abstraction.Services.PageService;
using ModCore.Abstraction.Themes;
using ModCore.Models.Page;
using ModCore.Specifications.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModCore.Core.Routers
{
    public class CmsPageRoute : IRouter
    {
        //private readonly string _controller; // get from object in database or manually set for 1 specific controller
        //private readonly string _action;
        private readonly IRouter _router;
        private readonly IDataRepositoryAsync<Page> _pageRepo;

        public CmsPageRoute(
            //string controller,
            //string action,
            //ICachedRouteDataProvider<TPrimaryKey> dataProvider,
            //IMemoryCache cache,
            IDataRepositoryAsync<Page> pageRepo,
             IRouter router)
        {
            //if (string.IsNullOrWhiteSpace(controller))
            //    throw new ArgumentNullException("controller");
            //if (string.IsNullOrWhiteSpace(action))
            //    throw new ArgumentNullException("action");
            //if (dataProvider == null)
            //    throw new ArgumentNullException("dataProvider");
            if (router == null)
                throw new ArgumentNullException("router");

            if (pageRepo == null)
                throw new ArgumentNullException("pageRepo");

            //_dataProvider = dataProvider;
            //_cache = cache;
            _router = router;
            _pageRepo = pageRepo;
            //// Set Defaults
            //CacheTimeoutInSeconds = 900;
            //_cacheKey = "__" + this.GetType().Name + "_GetPageList_" + _controller + "_" + _action;
        }

        public async Task RouteAsync(RouteContext context)
        {
            var requestPath = context.HttpContext.Request.Path.Value;

            if (!string.IsNullOrEmpty(requestPath) && requestPath[0] == '/')
            {
                // Trim the leading slash
                requestPath = requestPath.Substring(1);
            }

            // Get the page that matches.
            var page = await _pageRepo.FindAsync(new PageByUrl(requestPath));
            // If we got back a null value set, that means the URI did not match
            if (page != null)
            {
                var routeData = new RouteData();
                routeData.Values["controller"] = "Page";
                routeData.Values["action"] = "Index";
                routeData.Values["page"] = page;

                context.RouteData = routeData;
                await _router.RouteAsync(context);

            }

        }

        public VirtualPathData GetVirtualPath(VirtualPathContext context)
        {
            return null;
        }


    }
}
