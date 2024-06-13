using Project.BLL.DTOs.District;
using Project.DLL.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.Services.Interface
{
    public interface IDistrictRepository
    {
        Task<Result<List<DistrictGetDTOs>>> GetAll(CancellationToken cancellationToken = default);
        Task<Result<DistrictGetDTOs>> GetById(int DistrictId, CancellationToken cancellationToken = default);
        Task<Result<List<DistrictGetDTOs>>> GetByProvinceId(int ProvinceId, CancellationToken cancellationToken = default);
    }
}
