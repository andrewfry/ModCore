using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ModCore.Core.Controllers;
using ModCore.Abstraction.Site;
using ModCore.ViewModels.Base;
using AutoMapper;

namespace ModCore.Www.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : BaseController
    {
        public HomeController(ILog log,  ISiteSettingsManagerAsync siteSettingsManager,
            IBaseViewModelProvider baseModeProvider, IMapper mapper)
            : base(log,siteSettingsManager, baseModeProvider, mapper)
        {

        }

        public IActionResult Index()
        {
            var m = new BaseViewModel();


            return View(m);
        }

    }
}
