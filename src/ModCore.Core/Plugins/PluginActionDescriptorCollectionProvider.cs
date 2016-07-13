using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using ModCore.Abstraction.Plugins;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace ModCore.Core.Plugins
{
    public class PluginActionDescriptorCollectionProvider : IActionDescriptorCollectionProvider
    {
        private readonly IServiceProvider _serviceProvider;
        private ActionDescriptorCollection _collection;
        private IPluginManager _pluginManager;

        public PluginActionDescriptorCollectionProvider(IServiceProvider serviceProvider, IPluginManager pluginManager)
        {
            _serviceProvider = serviceProvider;
            _pluginManager = pluginManager;

        }

        public ActionDescriptorCollection ActionDescriptors
        {
            get
            {
                if (_collection == null)
                {
                    _collection = GetCollection();
                }

                return _collection;
            }
        }

        private ActionDescriptorCollection GetCollection()
        {
            var providers =
                _serviceProvider.GetServices<IActionDescriptorProvider>()
                                .OrderBy(p => p.Order)
                                .ToArray();

            var context = new ActionDescriptorProviderContext();

            foreach (var provider in providers)
            {
                provider.OnProvidersExecuting(context);
            }

            for (var i = providers.Length - 1; i >= 0; i--)
            {
                providers[i].OnProvidersExecuted(context);
            }

            return new ActionDescriptorCollection(
                new ReadOnlyCollection<ActionDescriptor>(context.Results), 0);
        }
    }
}
