using ModCore.Abstraction.Site;
using ModCore.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModCore.Core.Site
{
    public class DefaultBaseViewModelProvider : IBaseViewModelProvider
    {
        public BaseViewModel Update(IBaseController controller, BaseViewModel model)
        {
            //TODO - Update all of the values from here.

            return model;
        }

    }
}
