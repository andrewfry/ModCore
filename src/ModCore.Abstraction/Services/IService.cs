﻿using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ModCore.Abstraction.Services
{
    public interface IService<TEntity>
    {

        TEntity GetById(string id);

    }
}
