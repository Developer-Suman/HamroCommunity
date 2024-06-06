using Microsoft.EntityFrameworkCore;
using Project.BLL.DTOs.Authentication;
using Project.BLL.DTOs.Pagination;
using Project.DLL.Abstraction;

namespace Project.BLL.Services.Implementation
{
    public static class IQueryableExtension
    {

        //Extension Method applied
        //This method used only with IQuerable Data
        public static async Task<Result<PagedResult<T>>> ToPagedResultAsync<T>(this IQueryable<T> query, int pageIndex, int pageSize)
        {
            try
            {
                var totalItems = await query.CountAsync();
                var items = await query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();

                var pagedResult = new PagedResult<T>
                {
                    Items = items,
                    TotalItems = totalItems,
                    PageIndex = pageIndex,
                    PageSize = pageSize
                };

                return Result<PagedResult<T>>.Success(pagedResult);
                    
            }catch (Exception ex)
            {
                return Result<PagedResult<T>>.Failure("NotFound", "Getting proble while fetching data");
            }
        }

    }
}
