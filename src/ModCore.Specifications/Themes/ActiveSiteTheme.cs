using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;
using ModCore.Models.Themes;
using ModCore.Specifications.BuiltIns;
using ModCore.Abstraction.Themes;

namespace ModCore.Specifications.Themes
{
    public class ActiveSiteTheme : Specification<ActiveTheme>
    {      
        public ActiveSiteTheme()
        {

        }
        public override Expression<Func<ActiveTheme, bool>> IsSatisifiedBy()
        {
            return x => x.Id == "1";
        }
    }
}
