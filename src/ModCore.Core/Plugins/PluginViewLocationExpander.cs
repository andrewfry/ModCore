﻿using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Razor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModCore.Core.Plugins
{
    public class PluginViewLocationExpander : IViewLocationExpander
    {
        private const string PLUGIN_KEY = "PluginName";

        public void PopulateValues(ViewLocationExpanderContext context)
        {
            var contextItems = context.ActionContext.HttpContext.Items;
            context.Values[PLUGIN_KEY] = contextItems.ContainsKey(PLUGIN_KEY) ? contextItems[PLUGIN_KEY] as string : string.Empty;
        }

        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
        {
            string pluginName = null;
            if (context.Values.TryGetValue(PLUGIN_KEY, out pluginName))
            {
                viewLocations = new[] {
                $"/Plugins/{pluginName}.Plugin/Views/{{0}}.cshtml",
                $"/Plugins/{pluginName}.Plugin/Views/{{1}}/{{0}}.cshtml",
                $"/Plugins/{pluginName}.Plugin/Views/Shared/{{0}}.cshtml",
                     }.Concat(viewLocations);

                if (context.AreaName != null)
                {
                    viewLocations = new[] {
                    $"/Plugins/{pluginName}.Plugin/Areas/{context.AreaName}/Views/{context.ControllerName}/{{0}}.cshtml",
                    $"/Plugins/{pluginName}.Plugin/Areas/{context.AreaName}/Views/Shared/{{0}}.cshtml",
                    //$"/Plugins/{pluginName}.Plugin/{context.AreaName}/Views/{context.ControllerName}/{{0}}.cshtml",
                    //$"/Plugins/{pluginName}.Plugin/{context.AreaName}/Views/{{0}}.cshtml",
                    //$"/Plugins/{pluginName}.Plugin/Views/{context.AreaName}/{context.ControllerName}/{{0}}.cshtml",
                    }.Concat(viewLocations);
                }
            }


            return viewLocations;
        }
    }
}
