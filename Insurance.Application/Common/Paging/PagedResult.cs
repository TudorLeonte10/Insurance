using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Application.Common.Paging
{
    public class PagedResult<T>
    {
        public IReadOnlyList<T> Items { get; init; } = [];
        public int PageNumber { get; init; }
        public int PageSize { get; init; }
        public int TotalCount { get; init; }
    }

}
