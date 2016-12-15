using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.AspNetCore.Routing;
using ModCore.Abstraction.Plugins;
using Pages.Plugin.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pages.Plugin.Routers
{
    public class CmsPageRouter : IPluginRouter
    {
        //private readonly string _controller; // get from object in database or manually set for 1 specific controller
        //private readonly string _action;
        private readonly MvcRouteHandler _router;
        private readonly IPageService _pageService;

        public CmsPageRouter(
        //    //string controller,
        //    //string action,
        //    //ICachedRouteDataProvider<TPrimaryKey> dataProvider,
        //    //IMemoryCache cache,
            IPageService pageService,
            MvcRouteHandler router
            )
        {
            _router = router;
            _pageService = pageService;
        }

        //    //_dataProvider = dataProvider;
        //    //_cache = cache;
        //    _router = router;
        //    _pageService = pageService;
        //    //// Set Defaults
        //    //CacheTimeoutInSeconds = 900;
        //    //_cacheKey = "__" + this.GetType().Name + "_GetPageList_" + _controller + "_" + _action;
        //}

        public async Task RouteAsync(RouteContext context)
        {
            //_pageService = context.HttpContext.RequestServices.GetService(typeof(IPageService)) as IPageService;
            //_router = context.HttpContext.RequestServices.GetService(typeof(MvcRouteHandler)) as IRouter;
            //var _router = context.HttpContext.RequestServices.GetService(typeof(IRouteCollection)) as IRouter;
               // _router = routeBuilder.DefaultHandler;
               var requestPath = context.HttpContext.Request.Path.Value;
            if (!string.IsNullOrEmpty(requestPath) && requestPath[0] == '/')
            {
                // Trim the leading slash
                requestPath = requestPath.Substring(1);
            }

            // Get the page that matches.
            var page = await _pageService.GetPageByURL(requestPath);
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

        public int Position {
            get {
                return 1;
            }
        }


    }

}
