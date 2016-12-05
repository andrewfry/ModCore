using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace ModCore.Abstraction.DataAccess
{
    public interface IPagedResult<T>
    {
        int TotalResults { get; set; }

        int PageSize { get; set; }

        int CurrentPage { get; set; }

        int TotalPages { get; }

        IList<T> CurrentPageResults { get; }

    }
}
