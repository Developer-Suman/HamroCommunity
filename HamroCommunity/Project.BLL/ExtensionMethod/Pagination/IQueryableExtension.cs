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
        public static async Task<Result<PagedResult<T>>> ToPagedResultAsync<T>(this IQueryable<T> query, int? pageIndex, int? pageSize)
        {
            try
            {
                int validPageIndex = pageIndex ?? 1;
                int validPageSize = pageSize ?? 10;

                if (validPageIndex < 1)
                {
                    validPageIndex = 1;
                }

                if (validPageSize < 1)
                {
                    validPageSize = 10;
                }

                var totalItems = await query.CountAsync();
                var items = await query.Skip((validPageIndex - 1) * validPageSize).Take(validPageSize).ToListAsync();

                var pagedResult = new PagedResult<T>
                {
                    Items = items,
                    TotalItems = totalItems,
                    PageIndex = validPageIndex,
                    PageSize = validPageSize
                };

                return Result<PagedResult<T>>.Success(pagedResult);
                    
            }catch (Exception ex)
            {
                return Result<PagedResult<T>>.Failure("NotFound", "Getting proble while fetching data");
            }
        }

    }
}
