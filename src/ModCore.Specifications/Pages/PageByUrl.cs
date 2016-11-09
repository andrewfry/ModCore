using ModCore.Specifications.BuiltIns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ModCore.Models.Page;
using System.Linq.Expressions;

namespace ModCore.Specifications.Pages
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
            return x => x.FriendlyURL.ToLower() == _url.ToLower(); 
        }
    }
}
