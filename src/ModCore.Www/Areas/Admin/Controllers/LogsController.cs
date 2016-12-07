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
using ModCore.Abstraction.Services.Site;
using ModCore.Core.DataAccess;
using ModCore.Specifications.Site;
using ModCore.Abstraction.DataAccess;
using ModCore.Models.Core;
using ModCore.Models.Access;
using ModCore.Abstraction.Services.Access;

namespace ModCore.Www.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class LogsController : BaseController
    {
        private readonly ILogService _logService;
        private readonly IUserActivityService _userActivity;

        public LogsController(ILog log, ISiteSettingsManagerAsync siteSettingsManager,
            IBaseViewModelProvider baseModeProvider, IMapper mapper, ILogService logService, IUserActivityService userActivity)
            : base(log, siteSettingsManager, baseModeProvider, mapper)
        {
            _logService = logService;
            _userActivity = userActivity;
        }

        public async Task<IActionResult> Index(int page =1, int pageSize = 50)
        {
            var pagedRequest = new PagedRequest();
            pagedRequest.CurrentPage = page;
            pagedRequest.PageSize = pageSize;

            var specs = new List<ISpecification<Log>>();
            var spec = new AllLogs();
            specs.Add(spec);

            var result = await _logService.Filter(specs, pagedRequest);
            var returnView = _mapper.Map<vPagedResult<vLog>>(result);

            return View(returnView);
        }

        public async Task<IActionResult> Details(string id)
        {
            var log = await _logService.GetByIdAsync(id);
            var model = _mapper.Map<vLog>(log);

            return PartialView("_Details",model);
        }

        public async Task<IActionResult> UserActivity(int page = 1, int pageSize = 50)
        {
            var pagedRequest = new PagedRequest();
            pagedRequest.CurrentPage = page;
            pagedRequest.PageSize = pageSize;

            var specs = new List<ISpecification<UserActivity>>();
            var spec = new AllUserActivity();
            specs.Add(spec);

            var result = await _userActivity.Filter(specs, pagedRequest);
            var returnView = _mapper.Map<vPagedResult<vUserActivity>>(result);

            return View(returnView);
        }


    }
}
