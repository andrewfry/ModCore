// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.Core;
using Microsoft.AspNetCore.Mvc.Filters;
using ModCore.Abstraction.Plugins;
using System.Collections.Generic;

namespace ModCore.Core.Plugins

{
    public class PluginFilterProvider : IFilterProvider
    {
        private readonly IPluginManager _pluginManager;

        public PluginFilterProvider(IPluginManager pluginManager)
        {
            if (pluginManager == null)
            {
                throw new ArgumentNullException(nameof(pluginManager));
            }

            _pluginManager = pluginManager;
        }

        public int Order
        {
            get { return -1000; }
        }

        /// <inheritdoc />
        public void OnProvidersExecuting(FilterProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (context.ActionContext.ActionDescriptor.FilterDescriptors != null)
            {
                var filterCollection = _pluginManager.GetFilterDescriptorsForPlugins() as List<FilterDescriptor>;
                if (filterCollection != null)
                {
                    for (var i = 0; i < filterCollection.Count; i++)
                    {
                        var filter = filterCollection[i];
                        var filterItem = new FilterItem(filter);

                        ProvideFilter(context, filterItem);
                    }
                }
            }

        }

        /// <inheritdoc />
        public void OnProvidersExecuted(FilterProviderContext context)
        {
        }

        public virtual void ProvideFilter(FilterProviderContext context, FilterItem filterItem)
        {
            if (filterItem.Filter != null)
            {
                return;
            }

            var filter = filterItem.Descriptor.Filter;

            var filterFactory = filter as IFilterFactory;
            if (filterFactory == null)
            {
                filterItem.Filter = filter;
                filterItem.IsReusable = true;
            }
            else
            {
                var services = context.ActionContext.HttpContext.RequestServices;
                filterItem.Filter = filterFactory.CreateInstance(services);
                filterItem.IsReusable = filterFactory.IsReusable;

                if (filterItem.Filter == null)
                {
                    throw new InvalidOperationException();
                }

                ApplyFilterToContainer(filterItem.Filter, filterFactory);
            }
        }


        private void ApplyFilterToContainer(object actualFilter, IFilterMetadata filterMetadata)
        {
            Debug.Assert(actualFilter != null, "actualFilter should not be null");
            Debug.Assert(filterMetadata != null, "filterMetadata should not be null");

            var container = actualFilter as IFilterContainer;

            if (container != null)
            {
                container.FilterDefinition = filterMetadata;
            }
        }
    }
}
