using ModCore.Models.BaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModCore.Models.Communication
{
    public class EmailTemplate : BaseEntity
    {
        public string Name { get; set; }

        public string DisplayName { get; set; }

        public List<EmailContact> To { get; set; }

        public List<EmailContact> Cc { get; set; }

        public List<EmailContact> Bcc { get; set; }

        public string EmailLayoutId { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        public bool ReadReciept { get; set; }

        public object MailMessage { get; set; }

        public bool HasLayout()
        {
            return string.IsNullOrEmpty(this.EmailLayoutId) || string.IsNullOrWhiteSpace(this.EmailLayoutId);
        }

    }
}
