using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModCore.Core.Routers
{
    public class DebuggerRouteHandler : IRouter
    {
        private string _name;

        public DebuggerRouteHandler(string name)
        {
            _name = name;
        }

        public VirtualPathData GetVirtualPath(VirtualPathContext context)
        {
            throw new NotImplementedException();
        }

        public async Task RouteAsync(RouteContext context)
        {
            var routeValues = string.Join("", context.RouteData.Values);
            var message = String.Format("{0} Values={1} ", _name, routeValues);
            await context.HttpContext.Response.WriteAsync(message);
        }
    }
}
