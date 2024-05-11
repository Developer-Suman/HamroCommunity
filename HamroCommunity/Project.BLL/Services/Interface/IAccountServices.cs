using Project.BLL.DTOs.Authentication;
using Project.DLL.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.Services.Interface
{
    public interface IAccountServices
    {
        Task<Result<RegistrationCreateDTOs>> RegisterUser (RegistrationCreateDTOs userModel);
        Task<Result<TokenDTOs>> LoginUser (LogInDTOs logInDTOs);
        Task<Result<object>> LogoutUser(string userId);

        Task<Result<string>> CreateRoles(string rolename);
        Task<Result<AssignRolesDTOs>> AssignRoles(AssignRolesDTOs assignRolesDTOs);
        Task<Result<TokenDTOs>> GetNewToken(TokenDTOs tokenDTOs);
        Task<Result<List<RoleDTOs>>> GetAllRoles(int page, int pageSize, CancellationToken cancellationToken);
        Task<Result<List<UserDTOs>>> GetAllUsers(int page, int pageSize, CancellationToken cancellationToken);
        Task<Result<UserDTOs>> GetByUserId(string userId, CancellationToken cancellationToken);
    }
}
