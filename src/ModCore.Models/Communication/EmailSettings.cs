using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModCore.Models.Communication
{
    public class EmailSettings
    {

        public string EmailServer { get; set; }

        public int EmailServerPort { get; set; }

        public string EmailServerUserName { get; set; }

        public string EmailServerUserPassword { get; set; }

        public bool EmailServerSecure { get; set; }

        public bool TestMode { get; set; }

        public string TestEmailAddress { get; set; }

        public string LocalDomain { get; set; }

    }
}
