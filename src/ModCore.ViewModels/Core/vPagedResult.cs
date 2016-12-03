using ModCore.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModCore.ViewModels.Core
{
    public class vPagedResult<T> : BaseViewModel
    {
        public int TotalResults { get; set; }

        public int PageSize { get; set; }

        public int CurrentPage { get; set; }

        public int TotalPages { get; set; }

        public IList<T> CurrentPageResults { get; set; }
    }
}
