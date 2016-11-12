using ModCore.Abstraction.DataAccess;
using ModCore.Models.Access;
using ModCore.Models.BaseEntities;
using ModCore.Specifications.BuiltIns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ModCore.Specifications.Access
{
    public class GetByEmail : Specification<User>
    {
        private readonly string _email;

        public GetByEmail(string email)
        {
            this._email = email.ToLower();
        }

        public override Expression<Func<User, bool>> IsSatisifiedBy()
        {
            return a => a.EmailAddress.ToLower() == _email;
        }
    }
}
