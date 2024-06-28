using Microsoft.AspNetCore.Http;
using Project.BLL.DTOs.Citizenship;
using Project.BLL.DTOs.Pagination;
using Project.BLL.DTOs.UserData;
using Project.DLL.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.Services.Interface
{
    public interface IUserDataRepository
    {
        Task<Result<GetUserDataDTOs>> SaveUserData(CreateUserDataDTOs createUserDataDTOs, IFormFile imageUrl);
        Task<Result<GetUserDataDTOs>> GetUserDataById(string UserDataId, CancellationToken cancellationToken);
        Task<Result<GetUserDataDTOs>> UpdateUserData(string UserDataId, UpdateUserDataDTOs updateUserDataDTOs, IFormFile imageUrl);
        Task<Result<GetUserDataDTOs>> DeleteUserData(string UserDataId);
        Task<Result<PagedResult<GetUserDataDTOs>>> GetAllUserData(int page, int pageSize, CancellationToken cancellationToken);
    }
}
