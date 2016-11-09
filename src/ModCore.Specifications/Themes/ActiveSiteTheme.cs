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
    public class ActiveSiteTheme : Specification<SiteTheme>
    {      
        public ActiveSiteTheme()
        {

        }
        public override Expression<Func<SiteTheme, bool>> IsSatisifiedBy()
        {
            return x => x.Active == true;
        }
    }
}
