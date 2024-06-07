using Project.BLL.DTOs.Signiture;
using Project.DLL.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.Services.Interface
{
    public interface ISignitureRepository
    {
        Task<Result<SignitureGetDTOs>> GetById(string SignitureId, CancellationToken cancellationToken);
        Task<Result<SignitureGetDTOs>> SaveSigniture(SignitureCreateDTOs signitureCreateDTOs);
        Task<Result<SignitureGetDTOs>> UpdateSigniture(string SignitureId, SignitureUpdateDTOs signitureCreateDTOs);
        Task<Result<SignitureGetDTOs>> DeleteSigniture(string SignitureId);
        Task<Result<List<SignitureGetDTOs>>> GetAll(int pageIndex, int pageSize, CancellationToken cancellationToken);
    }
}
