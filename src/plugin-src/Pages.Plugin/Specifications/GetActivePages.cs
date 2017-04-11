using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ModCore.Specifications.BuiltIns;
using Pages.Plugin.Models;
using System.Linq.Expressions;

namespace Pages.Plugin.Specifications
{
    public class GetActivePages : Specification<Page>
    {
        public GetActivePages()
        {
            
        }

        public override Expression<Func<Page, bool>> IsSatisifiedBy()
        {
            return x => x.PageStatus == PageStatus.Active;
        }
    }
}
