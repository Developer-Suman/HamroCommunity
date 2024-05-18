using Project.BLL.DTOs.Nashu;
using Project.DLL.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.Services.Interface
{
    public interface INashuRepository
    {
        Task<Result<NashuGetDTOs>> SaveNashuData(NashuCreateDTOs nashuCreateDTOs);
        Task<Result<NashuGetDTOs>> GetNashuDataById(string NashuId, CancellationToken cancellationToken);
        Task<Result<NashuGetDTOs>> UpdateNashuData(string NashuId,NashuUpdateDTOs nashuUpdateDTOs);
        Task<Result<NashuGetDTOs>> DeleteNashuData(string NashuId);
        Task<Result<List<NashuGetDTOs>>> GetAllNashuData(int page, int pageSize, CancellationToken cancellationToken);
    }
}
