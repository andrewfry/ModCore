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
using ModCore.ViewModels.Core;
using Microsoft.AspNetCore.Http;

namespace ModCore.Www.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class LogsController : BaseController
    {
        private readonly ISiteSettingsManagerAsync _siteSettingsManager;

        public LogsController(ILog log, ISiteSettingsManagerAsync siteSettingsManager,
            IBaseViewModelProvider baseModeProvider, IMapper mapper)
            : base(log, siteSettingsManager, baseModeProvider, mapper)
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

      
    }
}
