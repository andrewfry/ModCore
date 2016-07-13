using ModCore.Abstraction.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModCore.Specifications.BuiltIns
{
    public static class ISpecificationExtensions
    {
        public static bool IsSatisfiedBy<T>(this ISpecification<T> specification, T entity)
        {
            return specification.IsSatisifiedBy().Compile()(entity);
        }

        public static ISpecification<T> And<T>(this ISpecification<T> left, ISpecification<T> right)
        {
            return SpecificationExtensions.And(left, right);
        }

        public static ISpecification<T> Or<T>(this ISpecification<T> left, ISpecification<T> right)
        {
            return SpecificationExtensions.Or(left, right);
        }
    }
}
