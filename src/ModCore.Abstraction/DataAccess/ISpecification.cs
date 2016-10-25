using System;
using System.Linq.Expressions;

namespace ModCore.Abstraction.DataAccess
{
    public interface ISpecification<T>
    {
        Expression<Func<T, bool>> IsSatisifiedBy();

        Func<T, bool> Predicate();

        Expression<Func<T, bool>> GetExpression();
    }
}
