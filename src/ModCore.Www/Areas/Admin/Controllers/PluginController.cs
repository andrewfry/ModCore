using System;
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
using Microsoft.AspNetCore.Http;
using ModCore.ViewModels.Core;

namespace ModCore.Www.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PluginController : BaseController
    {
        private IPluginManager _pluginManager;
        private IPluginSettingsManager _pluginSettingsManager;

        public PluginController(ILog log, ISiteSettingsManagerAsync siteSettingsManager, IBaseViewModelProvider baseModeProvider,
            IPluginManager pluginManager, IMapper mapper, ISessionService sessionService, IPluginSettingsManager pluginSettingsManager)
            : base(log, siteSettingsManager, baseModeProvider, mapper, sessionService)
        {
            _pluginManager = pluginManager;
            _pluginSettingsManager = pluginSettingsManager;
        }

        [HttpGet]
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

        [HttpGet]
        public IActionResult Settings(string pluginAssembly)
        {
            var plugin = _pluginManager.InstalledPlugins.FirstOrDefault(a => a.AssemblyName == pluginAssembly);
            _pluginSettingsManager.SetPlugin(plugin);

            var model = new vSettings();
            model.AssemblyName = plugin.AssemblyName;
            model.Name = plugin.Name;
            model.Settings = _pluginSettingsManager.GetAllAsync()
                .Result
                .Select(a => _mapper.Map<vSettingValue>(a)).ToList();

            return View(model);
        }


        public async Task<IActionResult> SaveSettingChanges(IFormCollection form)
        {
            var pluginAssembly = form["plugin_assembly"].ToString();
            var plugin = _pluginManager.InstalledPlugins.FirstOrDefault(a => a.AssemblyName.ToLower() == pluginAssembly.ToLower());
            _pluginSettingsManager.SetPlugin(plugin);

            try
            {
                foreach (var item in form)
                {
                    var settingPair = _pluginSettingsManager.GetSettingRegionPair(item.Key);
                    var contains = await _pluginSettingsManager.ContainsSettingAsync(settingPair);
                    if (contains)
                    {
                        await _pluginSettingsManager.UpsertSettingAsync(settingPair, item.Value[0]);
                    }
                }

                return JsonSuccess();
            }
            catch (Exception ex)
            {
                return JsonFail(ex.Message);
            }
        }
    }
}
