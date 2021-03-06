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
using ModCore.ViewModels.Core;
using Microsoft.AspNetCore.Http;
using ModCore.Abstraction.Services.Access;

namespace ModCore.Www.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SiteSettingsController : BaseController
    {
        private readonly ISiteSettingsManagerAsync _siteSettingsManager;

        public SiteSettingsController(ILog log, ISiteSettingsManagerAsync siteSettingsManager,
            IBaseViewModelProvider baseModeProvider, IMapper mapper, ISessionService sessionService)
            : base(log, siteSettingsManager, baseModeProvider, mapper, sessionService)
        {
            _siteSettingsManager = siteSettingsManager;
        }

        public IActionResult Index()
        {
            var model = new vSettings();
            model.Settings = _siteSettingsManager.GetAllAsync()
                .Result
                .Select(a => _mapper.Map<vSettingValue>(a)).ToList();

            return View(model);
        }

        public async Task<IActionResult> SaveChanges(IFormCollection form)
        {
            try
            {
                foreach (var item in form)
                {
                    var settingPair = _siteSettingsManager.GetSettingRegionPair(item.Key);
                    var contains = await _siteSettingsManager.ContainsSettingAsync(settingPair);
                    if (contains)
                    {
                        await _siteSettingsManager.UpsertSettingAsync(settingPair, item.Value[0]);
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
