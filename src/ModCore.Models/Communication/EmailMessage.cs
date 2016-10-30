using ModCore.Models.BaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModCore.Models.Communication
{
    public class EmailMessage
    {
        public string Name { get; set; }

        public string DisplayName { get; set; }

        public EmailContact From { get; set; }

        public List<EmailContact> To { get; set; }

        public List<EmailContact> Cc { get; set; }

        public List<EmailContact> Bcc { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

    }
}
