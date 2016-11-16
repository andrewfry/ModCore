using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ModCore.Core.Controllers;
using ModCore.Abstraction.Site;
using Microsoft.Extensions.Logging;
using ModCore.ViewModels.Base;
using AutoMapper;

namespace ModCore.Www.Controllers
{
    public class CacheController : BaseController
    {
        public CacheController(ILog log, ISessionManager sessionManager, ISiteSettingsManagerAsync siteSettingsManager,
            IBaseViewModelProvider baseModeProvider, IMapper mapper)
            : base(log,sessionManager,siteSettingsManager, baseModeProvider, mapper)
        {

        }

        public IActionResult Invalidate()
        {
            //TODO -    Invalidate the various caching mechanisms
            //          This will be used for multiple servers

            return Ok(new
            {
                CacheStatus = "Cleared"
            });
        }

        
    }
}
