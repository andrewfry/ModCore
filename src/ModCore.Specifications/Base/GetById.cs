using ModCore.Abstraction.DataAccess;
using ModCore.Models.BaseEntities;
using ModCore.Specifications.BuiltIns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ModCore.Specifications.Base
{
    public class GetById<T> : Specification<T> where T : BaseEntity
    {
        private readonly string _id;

        public GetById(string id)
        {
            this._id = id;
        }

        public override Expression<Func<T, bool>> IsSatisifiedBy()
        {
            return a => a.Id == _id;
        }
    }
}
