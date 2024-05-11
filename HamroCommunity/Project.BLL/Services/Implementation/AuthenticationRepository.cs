using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Project.BLL.DTOs.Authentication;
using Project.BLL.Services.Interface;
using Project.BLL.Static.Cache;
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
        private readonly IMemoryCacheRepository _memoryCacheRepository;

        public AuthenticationRepository(UserManager<ApplicationUsers> userManager, RoleManager<IdentityRole> roleManager, IMapper mapper, IMemoryCacheRepository memoryCacheRepository)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _memoryCacheRepository = memoryCacheRepository;
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

        public async Task<List<RoleDTOs>> GetAllRolesAsync(int page, int pageSize, CancellationToken cancellationToken)
        {
            var roles = await _roleManager.Roles
                .Where(x => x.Name != "superadmin")
                .AsNoTracking()
                .Skip((page - 1)*pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);
            if(roles != null && roles.Count() > 0)
            {
                var rolesList = _mapper.Map<List<RoleDTOs>>(roles);
                return rolesList;
            }
            return new List<RoleDTOs>();

            //Ternery Operator
            //return roles != null && roles.Any()
            //    ? _mapper.Map<List<RoleDTOs>>(roles)
            //    : new List<RoleDTOs>();

        }


        public async Task<List<UserDTOs>> GetAllUsersAsync(int page, int pageSize, CancellationToken cancellationToken)
        {
            var cacheKey = CacheKeys.User;
            var cacheData = await _memoryCacheRepository.GetCacheKey<List<UserDTOs>>(cacheKey);

            if(cacheData is not null && cacheData.Count > 0)
            {
                return cacheData;
            }

            var users = await _userManager.Users.AsNoTracking()
                .OrderByDescending(x=>x.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            await _memoryCacheRepository.SetAsync(cacheKey, users, new Microsoft.Extensions.Caching.Memory.MemoryCacheEntryOptions
            {
                AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(30)
            }, cancellationToken) ;

            var userDTOs = _mapper.Map<List<UserDTOs>>(users);
            return userDTOs;
        }

        public async Task<UserDTOs> GetById(string id, CancellationToken cancellationToken)
        {
            var cacheKey = $"GetById{id}";
            var cacheData = await _memoryCacheRepository.GetCacheKey<UserDTOs>(cacheKey);
            if(cacheData is not null)
            {
                return cacheData;
            }

            var user = await _userManager.FindByIdAsync(id);
            if(user is null)
            {
                return default!;
            }

            var userDTOs = _mapper.Map<UserDTOs>(user);

            await _memoryCacheRepository.SetAsync(cacheKey, userDTOs, new Microsoft.Extensions.Caching.Memory.MemoryCacheEntryOptions
            {
                AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(5)
            }, cancellationToken);

            return userDTOs;
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
