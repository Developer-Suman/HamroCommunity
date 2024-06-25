using Project.BLL.DTOs.Branch;
using Project.BLL.DTOs.Department;
using Project.BLL.DTOs.Pagination;
using Project.DLL.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.Services.Interface
{
    public interface IDepartmentRepository
    {
        Task<Result<DepartmentGetDTOs>> SaveDepartment(DepartmentCreateDTOs departmentCreateDTOs);
        Task<Result<DepartmentGetDTOs>> GetById(string DepartmentId, CancellationToken cancellationToken);
        Task<Result<DepartmentGetDTOs>> DeleteDepartment(string DepartmentId, CancellationToken cancellationToken);
        Task<Result<DepartmentGetDTOs>> UpdateDepartment(string DepartmentId, DepartmentUpdateDTOs branchUpdateDTOs);
        Task<Result<PagedResult<DepartmentGetDTOs>>> GetAll(int pageIndex, int pageSize, CancellationToken cancellationToken);
    }
}
