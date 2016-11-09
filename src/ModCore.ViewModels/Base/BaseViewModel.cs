using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModCore.ViewModels.Base
{
    public class BaseViewModel
    {
        public string Token { get; set; }

        public string UserId { get; set; }

        public bool IsLoggedIn { get; set; }

        public vUserData UserData { get; set; }

        public vSiteSettings SiteSettings { get; set; }

    }
}
