using ModCore.Models.BaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModCore.Models.Access
{
    public class User : BaseEntity
    {

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string EmailAddress { get; set; }

        public string PasswordHash { get; set; }

        public string PasswordSalt { get; set; }

        public string EmailHashVerification { get; set; }

        public bool EmailVerified { get; set; }

        public DateTime LastLogin { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime? LastPasswordReset { get; set; }

        public DateTime LastUpdateDate { get; set; }

        public int FailedLoginAttempts { get; set; }

        public bool LockedOut { get; set; }

        public User()
        {
            this.EmailVerified = false;
        }

    }
}
