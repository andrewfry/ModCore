using ModCore.Abstraction.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModCore.Core.DataAccess
{
    public class PagedResult<T> : IPagedResult<T>
    {
        public int TotalResults { get; set; }

        public int PageSize { get; set; }

        public int CurrentPage { get; set; }

        public int TotalPages { get { return (TotalResults / PageSize) + 1; } }

        public IList<T> CurrentPageResults { get; set; }

    }
}
