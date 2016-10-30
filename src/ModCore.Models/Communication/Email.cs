using ModCore.Models.BaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModCore.Models.Communication
{
    public class Email : BaseEntity
    {
        public string Name { get; set; }

        public string DisplayName { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        public bool ReadReciept { get; set; }

        public object MailMessage { get; set; }

    }
}
