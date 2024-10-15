using Microsoft.EntityFrameworkCore;

namespace CourseManager.Domain.DTOs
{
    public class PageResponse<T>
    {
        public List<T> Items { get; set; }
        public int TotalItems { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; private set; }

        public PageResponse(List<T> itemList, int count, int pageIndex, int pageSize)
        {
            Items = itemList;
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            TotalItems = count;
            PageSize = pageSize;
        }

        public bool HasPreviousPage => PageIndex > 1;
        public bool HasNextPage => PageIndex < TotalPages;

        public static async Task<PageResponse<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
        {
            var count = await source.CountAsync();
            var itemList = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PageResponse<T>(itemList, count, pageIndex, pageSize);
        }
    }
}
