using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModCore.Abstraction.DataAccess
{
    public interface IPagedRequest
    {
        int? TotalResults { get; set; }

        int PageSize { get; set; }

        int CurrentPage { get; set; }
    }
}
