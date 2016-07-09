using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModCore.ViewModels.Base
{
    public class BaseModel
    {
        public string Token { get; set; }

        public string UserId { get; set; }

        public UserData UserData { get; set; }

        public SiteSettings SiteSettings { get; set; }

    }
}
