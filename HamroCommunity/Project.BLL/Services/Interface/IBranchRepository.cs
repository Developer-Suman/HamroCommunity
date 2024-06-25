using Microsoft.AspNetCore.Http;
using Project.BLL.DTOs.Branch;
using Project.BLL.DTOs.CertificateDTOs;
using Project.BLL.DTOs.Pagination;
using Project.DLL.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.Services.Interface
{
    public interface IBranchRepository
    {
        Task<Result<BranchGetDTOs>> SaveBranch(BranchCreateDTOs branchCreateDTOs);
        Task<Result<BranchGetDTOs>> GetById(string BranchId, CancellationToken cancellationToken);
        Task<Result<BranchGetDTOs>> DeleteBranch(string BranchId, CancellationToken cancellationToken);
        Task<Result<BranchGetDTOs>> UpdateBranch(string BranchId, BranchUpdateDTOs branchUpdateDTOs);
        Task<Result<PagedResult<BranchGetDTOs>>> GetAll(int pageIndex, int pageSize, CancellationToken cancellationToken);
    }
}
