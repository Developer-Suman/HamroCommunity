using Microsoft.AspNetCore.Http;
using Project.BLL.DTOs.Citizenship;
using Project.BLL.DTOs.Nashu;
using Project.BLL.DTOs.Pagination;
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
        Task<Result<CitizenshipGetDTOs>> SaveCitizenshipData(CitizenshipCreateDTOs citizenshipCreateDTOs, List<IFormFile> certificateFiles);
        Task<Result<CitizenshipGetDTOs>> GetCitizenshipDataById(string CitizenshipId, CancellationToken cancellationToken);
        Task<Result<CitizenshipGetDTOs>> UpdateCitizenshipData(string CitizenshipId, CitizenshipUpdateDTOs citizenshipUpdateDTOs, List<IFormFile> multipleFiles);
        Task<Result<CitizenshipGetDTOs>> DeleteCitizenshipData(string CitizenshipId);
        Task<Result<PagedResult<CitizenshipGetDTOs>>> GetAllCitizenshipData(int page, int pageSize, CancellationToken cancellationToken);
    }
}
