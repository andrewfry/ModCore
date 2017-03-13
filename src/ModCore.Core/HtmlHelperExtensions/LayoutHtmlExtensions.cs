using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using ModCore.Abstraction.Site;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModCore.Core.HtmlHelperExtensions
{
    public static class LayoutHtmlExtensions
    {
        public static string DefaultLayout(this IHtmlHelper html)
        {
            var layoutService = html.ViewContext.HttpContext.RequestServices.GetService(typeof(ILayoutManager)) as ILayoutManager;

            return layoutService.DefaultLayoutPath;
        }
    }
}
