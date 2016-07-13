using ModCore.Abstraction.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ModCore.Specifications.BuiltIns
{
    public abstract class Specification<T> : ISpecification<T>
    {
        public abstract Expression<Func<T, bool>> IsSatisifiedBy();
       
        public Func<T, bool> Predicate()
        {
            Func<T, bool> func = IsSatisifiedBy().Compile();
            //Predicate<T> pred = t => func(t);
            return func;
        }


    }
}
