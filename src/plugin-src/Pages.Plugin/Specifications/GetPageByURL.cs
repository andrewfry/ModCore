using ModCore.Specifications.BuiltIns;
using Pages.Plugin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Pages.Plugin.Specifications
{
    public class PageByUrl : Specification<Page>
    {
        private readonly string _url;
        public PageByUrl(string url)
        {
            _url = url;
        }

        public override Expression<Func<Page, bool>> IsSatisifiedBy()
        {
            return x => x.Url.ToLower() == _url.ToLower();
        }
    }
}
