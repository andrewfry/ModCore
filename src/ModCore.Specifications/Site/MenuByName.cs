﻿using ModCore.Abstraction.DataAccess;
using ModCore.Models.BaseEntities;
using ModCore.Models.Core;
using ModCore.Models.Site;
using ModCore.Specifications.BuiltIns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ModCore.Specifications.Site
{
    public class MenuByName : Specification<Menu>
    {
        private string _name;

        public MenuByName(string name)
        {
            _name = name;
        }

        public override Expression<Func<Menu, bool>> IsSatisifiedBy()
        {
            return a => a.Name.ToLower() == _name.ToLower();
        }
    }
}
