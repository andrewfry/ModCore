using Microsoft.AspNetCore.Http;
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
            if (model == null)
                throw new ArgumentNullException("model");

            var s = controller.CurrentSession;

            if (s.IsLoggedIn)
            {
                model.IsLoggedIn = s.IsLoggedIn;
                model.UserId = s.UserId;
                model.UserData = new vUserData
                {
                    EmailAddress = s.UserData.EmailAddress,
                    FirstName = s.UserData.FirstName,
                    LastName = s.UserData.LastName,
                };
            }

            return model;
        }

    }
}
