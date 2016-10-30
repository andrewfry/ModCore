using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModCore.Models.Communication
{
    public class EmailContact
    {
        public string EmailAddress { get; set; }

        public string DisplayName { get; set; }

        public EmailContact(string emailAddress)
        {
            this.EmailAddress = emailAddress;
        }

        public EmailContact(string displayName, string emailAddress)
        {
            this.EmailAddress = emailAddress;
            this.DisplayName = displayName;
        }

        public bool JustEmail()
        {
            if (string.IsNullOrEmpty(DisplayName) || string.IsNullOrWhiteSpace(DisplayName))
                return true;

            return false;
        }

    }

}
