﻿using System;
using System.Linq.Expressions;

namespace ModCore.Abstraction.DataAccess
{
    public interface ISpecification<T>
    {
        Expression<Func<T, bool>> IsSatisifiedBy();
    }
}
