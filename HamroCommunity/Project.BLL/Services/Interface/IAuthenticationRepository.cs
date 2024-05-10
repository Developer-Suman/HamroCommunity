using Microsoft.AspNetCore.Identity;
using Project.BLL.DTOs.Authentication;
using Project.DLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.Services.Interface
{
    public interface IAuthenticationRepository
    {
        Task<ApplicationUsers> FindByNameAsync(string userName);
        Task<ApplicationUsers> FindByIdAsync(string id);
        Task<ApplicationUsers> FindByEmailAsync(string email);
        Task<IList<string>> GetRolesAsync(ApplicationUsers username);
        Task<bool> CheckPasswordAsync(ApplicationUsers username, string password);
        Task<IdentityResult> CreateUserAsync(ApplicationUsers user, string password);
        Task UpdateUserAsync(ApplicationUsers user);
        Task<IdentityResult> CreateRoles(string roles);
        Task<IdentityResult> AssignRoles(ApplicationUsers user, string rolename);
        Task<bool> CheckRolesAsync(string role);
        Task<List<UserDTOs>> GetAllUsersAsync(int page, int pageSize, CancellationToken cancellationToken);
        Task<UserDTOs> GetById(string  id, CancellationToken cancellationToken);
    }
}
