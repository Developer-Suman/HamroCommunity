using Project.BLL.DTOs.Branch;
using Project.BLL.DTOs.Nijamati;
using Project.BLL.DTOs.Pagination;
using Project.DLL.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.Services.Interface
{
    public interface INijamatiRepository
    {
        Task<Result<NijamatiGetDTOs>> SaveNijamati(NijamatiCreateDTOs nijamatiCreateDTOs);
        Task<Result<NijamatiGetDTOs>> GetById(string NijamatiId, CancellationToken cancellationToken);
        Task<Result<NijamatiGetDTOs>> DeleteNijamati(string NijamatiId, CancellationToken cancellationToken);
        Task<Result<NijamatiGetDTOs>> UpdateNijamati(string NijamatiId, NijamatiUpdateDTOs branchUpdateDTOs);
        Task<Result<PagedResult<NijamatiGetDTOs>>> GetAll(int pageIndex, int pageSize, CancellationToken cancellationToken);
    }
}
