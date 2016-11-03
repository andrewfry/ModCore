using ModCore.Abstraction.DataAccess;
using ModCore.Models.BaseEntities;
using ModCore.Models.Objects;
using ModCore.Specifications.BuiltIns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ModCore.Specifications.Objects
{
    public class ObjectByName : Specification<ObjectDefinition> 
    {
        private readonly string _name;

        public ObjectByName(string name)
        {
            this._name = name;
        }

        public override Expression<Func<ObjectDefinition, bool>> IsSatisifiedBy()
        {
            return a => a.ObjectName.ToLower() == _name.ToLower();
        }
    }
}
