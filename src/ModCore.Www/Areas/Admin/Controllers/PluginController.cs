﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ModCore.Core.Controllers;
using ModCore.Abstraction.Site;
using Microsoft.Extensions.Logging;
using ModCore.ViewModels.Base;
using ModCore.Abstraction.Plugins;
using ModCore.ViewModels.Admin.Plugin;
using ModCore.Core.Plugins;
using AutoMapper;
using ModCore.Core.Site;
using ModCore.Abstraction.Services.Access;
using ModCore.Models.Plugins;

namespace ModCore.Www.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PluginController : BaseController
    {
        private IPluginManager _pluginManager;

        public PluginController(ILog log, ISiteSettingsManagerAsync siteSettingsManager,
            IBaseViewModelProvider baseModeProvider, IPluginManager pluginManager, IMapper mapper, ISessionService sessionService)
            : base(log, siteSettingsManager, baseModeProvider, mapper, sessionService)
        {
            _pluginManager = pluginManager;
        }

        public IActionResult Index()
        {

            var availablePlugins = _pluginManager.AvailablePlugins.Select(a => _mapper.Map<vPlugin>(a)).ToList();
            var installedPlugins = _pluginManager.InstalledPlugins.ToList();
            var activePlugins = _pluginManager.ActivePlugins.ToList();

            foreach (var avail in availablePlugins)
            {
                avail.Installed = installedPlugins.Any(a => a.AssemblyName == avail.AssemblyName && avail.Version == a.Version);
                avail.Active = activePlugins.Any(a => a.AssemblyName == avail.AssemblyName && avail.Version == a.Version);
            }

            var m = new vPluginList();
            m.Plugins = availablePlugins;
            m.PluginErrors = _pluginManager.Errors.Where(a => a.ErrorLevel == PluginErrorLevel.Error).ToList();
            m.PluginWarnings = _pluginManager.Errors.Where(a => a.ErrorLevel == PluginErrorLevel.Warning).ToList();

            return View(m);
        }


        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult EnabledPlugin(string pluginAssembly)
        {
            var plugin = _pluginManager.AvailablePlugins.FirstOrDefault(a => a.AssemblyName == pluginAssembly);

            if (plugin == null)
                return JsonFail($"Can not find the plugin {pluginAssembly}");

            _pluginManager.ActivatePlugin(plugin);

            var appManager = ApplicationManager.Load();
            appManager.Restart();

            return JsonSuccess();
        }


        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult DisablePlugin(string pluginAssembly)
        {
            var plugin = _pluginManager.InstalledPlugins.FirstOrDefault(a => a.AssemblyName == pluginAssembly);

            if (plugin == null)
                return JsonFail($"Can not find the plugin {pluginAssembly}");

            _pluginManager.DeactivatePlugin(plugin);

            var appManager = ApplicationManager.Load();
            appManager.Restart();

            return JsonSuccess();
        }
    }
}
