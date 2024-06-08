using Project.BLL.DTOs.Citizenship;
using Project.BLL.DTOs.Nashu;
using Project.DLL.Abstraction;
using Project.DLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.Services.Interface
{
    public interface ICitizenshipRepository
    {
        Task<Result<CitizenshipGetDTOs>> SaveCitizenshipData(CitizenshipCreateDTOs citizenshipCreateDTOs, List<CitizenshipImagesDTOs> citizenshipImages);
        Task<Result<CitizenshipGetDTOs>> GetCitizenshipDataById(string CitizenshipId, CancellationToken cancellationToken);
        Task<Result<CitizenshipGetDTOs>> UpdateCitizenshipData(string CitizenshipId, CitizenshipUpdateDTOs citizenshipUpdateDTOs);
        Task<Result<CitizenshipGetDTOs>> DeleteCitizenshipData(string CitizenshipId);
        Task<Result<List<CitizenshipGetDTOs>>> GetAllCitizenshipData(int page, int pageSize, CancellationToken cancellationToken);
    }
}
