using Project.BLL.DTOs.Municipality;
using Project.DLL.Abstraction;
using Project.DLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.Services.Interface
{
    public interface IMunicipalityRepository
    {
        Task<Result<MunicipalityGetDTOs>> GetById(string municipalityId, CancellationToken cancellationToken = default);
        Task<Result<List<MunicipalityGetDTOs>>> GetAll(CancellationToken cancellationToken =default);
        Task<Result<List<MunicipalityGetDTOs>>> GetByDistrictId(string districtId, CancellationToken cancellationToken = default);
    }
}
