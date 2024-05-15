using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Project.DLL.DbContext;
using Project.DLL.RepoInterface;
using Project.DLL.Static.Roles;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Project.DLL.Seed
{
    public class DataSeeder
    {
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUnitOfWork _unitOfWork;

        public DataSeeder(RoleManager<IdentityRole> roleManager, ApplicationDbContext applicationDbContext, IUnitOfWork unitOfWork)
        {
            _roleManager = roleManager;
            _unitOfWork = unitOfWork;
            _context = applicationDbContext;
            
        }

        public async Task Seed()
        {
            using (var scode = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    await SeedRole();

                    scode.Complete();

                }catch(Exception ex)
                {
                    scode.Dispose();
                    throw;

                }
            }
        }

        #region Roles
        private async Task SeedRole()
        {
            if(!await _roleManager.Roles.AnyAsync())
            {
                var roles = new List<IdentityRole>()
                {
                     new IdentityRole(){Name = Role.BranchAdmin},
                     new IdentityRole(){Name = Role.Superadmin},
                     new IdentityRole(){Name = Role.DepartmentAdmin}
                };

                foreach(var role in roles)
                {
                    await _roleManager.CreateAsync(role);
                }
            }
        }

        #endregion
    }
}
