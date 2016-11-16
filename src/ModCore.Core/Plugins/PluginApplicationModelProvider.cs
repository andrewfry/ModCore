using Microsoft.AspNetCore.Mvc.ApplicationModels;
using ModCore.Abstraction.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModCore.Core.Plugins
{
    public class PluginApplicationModelProvider : IApplicationModelProvider
    {
        private readonly IPluginManager _pluginManager;

        public PluginApplicationModelProvider(IPluginManager pluginManager)
        {
            if (pluginManager == null)
            {
                throw new ArgumentNullException(nameof(pluginManager));
            }

            _pluginManager = pluginManager;
        }

        public int Order { get { return -1000 + 20; } }

        public void OnProvidersExecuted(ApplicationModelProviderContext context)
        {
            // Intentionally empty.
        }

        public void OnProvidersExecuting(ApplicationModelProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            foreach (var controllerModel in context.Result.Controllers)
            {
                var filterCollection = _pluginManager.GetFilterDescriptorsForPlugins();
                if (filterCollection != null)
                {
                    for (var i = 0; i < filterCollection.Count; i++)
                    {
                        var filter = filterCollection[i];
                        
                        controllerModel.Filters.Add(filter.Filter);
                    }
                }

                //foreach (var attribute in controllerModel.Attributes.OfType<IAllowAnonymous>())
                //{
                //    controllerModel.Filters.Add(new AllowAnonymousFilter());
                //}

                //foreach (var actionModel in controllerModel.Actions)
                //{
                //    var actionModelAuthData = actionModel.Attributes.OfType<IAuthorizeData>().ToArray();
                //    if (actionModelAuthData.Length > 0)
                //    {
                //        actionModel.Filters.Add(GetFilter(_policyProvider, actionModelAuthData));
                //    }

                //    foreach (var attribute in actionModel.Attributes.OfType<IAllowAnonymous>())
                //    {
                //        actionModel.Filters.Add(new AllowAnonymousFilter());
                //    }
                //}
            }
        }

    }
}
