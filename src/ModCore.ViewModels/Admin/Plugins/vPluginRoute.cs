using ModCore.ViewModels.Admin.Plugin;
using ModCore.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModCore.ViewModels.Admin.Plugin
{
    public class vPluginRoutes : BaseViewModel
    {
        public IList<vRoute> Routes { get; set; }
    }

    public class vRoute
    {
        public string RouteName { get; set; }

        public string RouteTemplate { get; set; }

        public string PluginName { get; set; }

        public string PluginVersion { get; set; }
    }
}
