using Project.BLL.DTOs.Municipality;
using Project.BLL.DTOs.Vdc;
using Project.DLL.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.Services.Interface
{
    public interface IVDCRepository
    {
        Task<Result<VdcGetDTOs>> GetById(int VdcId, CancellationToken cancellationToken = default);
        Task<Result<List<VdcGetDTOs>>> GetAll(CancellationToken cancellationToken = default);
        Task<Result<List<VdcGetDTOs>>> GetByDistrictId(int districtId, CancellationToken cancellationToken = default);
    }
}
