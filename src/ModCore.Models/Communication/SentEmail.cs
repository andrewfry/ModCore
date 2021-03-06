﻿using ModCore.Models.BaseEntities;
using ModCore.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModCore.Models.Communication
{
    public class SentEmail : BaseEntity
    {
        public string Name { get; set; }

        public EmailContact From { get; set; }

        public List<EmailContact> To { get; set; }

        public List<EmailContact> Cc { get; set; }

        public List<EmailContact> Bcc { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        public DateTime DateSent { get; set; }

        public EmailSentStatus Status { get; set; }
    }
}
