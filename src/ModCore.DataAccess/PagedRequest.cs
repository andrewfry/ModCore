using ModCore.Abstraction.DataAccess;

namespace ModCore.Core.DataAccess
{
    public class PagedRequest : IPagedRequest
    {
        public int? TotalResults { get; set; }

        public int PageSize { get; set; }

        public int CurrentPage { get; set; }

        public PagedRequest()
        {
            TotalResults = null;
        }
    }
}
