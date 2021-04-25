using System;
using System.Collections.Generic;
using System.Linq;

namespace NeedsSoySauce.Data
{
    public class PagedResult<T>
    {
        public int Pages { get; set; }
        public int PageSize { get; set; }
        public int Page { get; set; }
        public bool HasNext { get; set; }
        public bool HasPrevious { get; set; }
        public IList<T> Items { get; set; }
        public PagedResult(IList<T> items)
        {
            Items = items;
        }
    }

    public static class IQueryableExtensions
    {
        public static PagedResult<T> Paginate<T>(this IQueryable<T> query, int page, int pageSize)
        {
            if (page < 1) throw new ArgumentOutOfRangeException(nameof(page));
            if (pageSize < 1) throw new ArgumentOutOfRangeException(nameof(pageSize));

            var rows = query.Count();
            var items = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var pages = (int)Math.Ceiling((double)rows / pageSize);

            return new PagedResult<T>(items)
            {
                Pages = pages,
                PageSize = pageSize,
                Page = page,
                HasNext = page < pages,
                HasPrevious = page > 1
            };
        }
    }
}