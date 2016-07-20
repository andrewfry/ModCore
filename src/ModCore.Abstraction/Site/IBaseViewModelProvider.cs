using Microsoft.AspNetCore.Http;
using ModCore.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModCore.Abstraction.Site
{
    public interface IBaseViewModelProvider
    {

        BaseViewModel Update(IBaseController controller, BaseViewModel model);

    }
}
