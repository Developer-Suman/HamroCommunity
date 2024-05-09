using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Project.BLL.DTOs;
using Project.BLL.Services.Interface;
using Project.DLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.Services.Implementation
{
    public class AuthenticationRepository : IAuthenticationRepository
    {
        private readonly UserManager<ApplicationUsers> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;

        public AuthenticationRepository(UserManager<ApplicationUsers> userManager, RoleManager<IdentityRole> roleManager, IMapper mapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
            
        }
        public async Task<IdentityResult> AssignRoles(ApplicationUsers user, string rolename)
        {
            return await _userManager.AddToRoleAsync(user, rolename);
        }

        public async Task<bool> CheckPasswordAsync(ApplicationUsers username, string password)
        {
            return await _userManager.CheckPasswordAsync(username, password);
        }

        public Task<bool> CheckRolesAsync(string role)
        {
            return _roleManager.RoleExistsAsync(role);
        }

        public async Task<IdentityResult> CreateRoles(string roles)
        {
            return await _roleManager.CreateAsync(new IdentityRole(roles));
        }

        public async Task<IdentityResult> CreateUserAsync(ApplicationUsers user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public async Task<ApplicationUsers> FindByEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if(user is null)
            {
                return default!;
            }
            return user;
        }

        public async Task<ApplicationUsers> FindByIdAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if(user is null)
            {
                return default!;
            }
            return user;
        }

        public async Task<ApplicationUsers> FindByNameAsync(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if(user is null)
            {
                return default!;
            }
            return user;
        }

        public Task<List<UserDTOs>> GetAllUsersAsync(int page, int pageSize, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<UserDTOs> GetById(string id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<IList<string>> GetRolesAsync(ApplicationUsers username)
        {
            return await _userManager.GetRolesAsync(username);
        }

        public Task UpdateUserAsync(ApplicationUsers user)
        {
            return _userManager.UpdateAsync(user);
        }
    }
}
