using ModCore.Abstraction.DataAccess;
using ModCore.Models.BaseEntities;
using ModCore.Models.Core;
using ModCore.Specifications.BuiltIns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ModCore.Specifications.Site
{
    public class GetSettingByName : Specification<SiteSetting>
    {
        private readonly string _name;

        public GetSettingByName(string name)
        {
            this._name = name;
        }

        public override Expression<Func<SiteSetting, bool>> IsSatisifiedBy()
        {
            return a => a.Name.ToLower() == _name.ToLower();
        }
    }
}
