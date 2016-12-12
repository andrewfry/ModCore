using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ModCore.Core.Controllers;
using ModCore.Abstraction.Site;
using ModCore.ViewModels.Base;
using AutoMapper;
using ModCore.Services.Access;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using ModCore.Abstraction.Services.Access;

namespace ModCore.Www.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : BaseController
    {
        public HomeController(ILog log, ISiteSettingsManagerAsync siteSettingsManager,
            IBaseViewModelProvider baseModeProvider, IMapper mapper, ISessionService sessionService)
            : base(log, siteSettingsManager, baseModeProvider, mapper, sessionService)
        {

        }

        public IActionResult Index()
        {
            var m = new BaseViewModel();

           // var srcToAdd = HttpContext.RequestServices.GetService(typeof(IActionDescriptorCollectionProvider));
           // var srv = new PermissionService(srcToAdd as IActionDescriptorCollectionProvider);

            //srv.GetAllDiscriptor();

            return View(m);
        }

    }
}
