using ModCore.Models.Communication;
using ModCore.Specifications.BuiltIns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ModCore.Specifications.Communication
{

    public class EmailByName : Specification<Email> 
    {
        private readonly string _name;

        public EmailByName(string name)
        {
            this._name = name;
        }

        public override Expression<Func<Email, bool>> IsSatisifiedBy()
        {
            return a => a.Name.ToLower() == _name.ToLower();
        }
    }
}
