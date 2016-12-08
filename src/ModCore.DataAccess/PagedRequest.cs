using ModCore.Abstraction.DataAccess;
using System;
using System.Linq.Expressions;

namespace ModCore.Core.DataAccess
{
    public class PagedRequest<T> : IPagedRequest
    {
        public int? TotalResults { get; set; }

        public int PageSize { get; set; }

        public int CurrentPage { get; set; }

        public OrderByRequest<T>? OrderByRequest { get; private set; }

        public void OrderBy(Expression<Func<T, dynamic>> orderBy, bool ascending)
        {
            var orderRequest = new OrderByRequest<T>();
            orderRequest.OrderBy = orderBy;
            orderRequest.Ascending = ascending;

            OrderByRequest = orderRequest;
        }

        public PagedRequest()
        {
            TotalResults = null;
            OrderByRequest = null;
        }
    }


    public struct OrderByRequest<T>
    {
        public Expression<Func<T, dynamic>> OrderBy { get; set; }

        public bool Ascending { get; set; }
    }
}
