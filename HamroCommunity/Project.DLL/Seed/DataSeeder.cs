using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Project.DLL.DbContext;
using Project.DLL.Models;
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
                    await SeedProvince();

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

        #region Province
        private async Task SeedProvince()
        {
            if (!await _context.Provinces.AnyAsync())
            {
                var provinces = new List<Province>()
            {
                new Province(1,"कोशी","Koshi Province"),
                new Province(2,"मधेश प्रदेश","Madhesh Province"),
                new Province(3,"बाग्मती प्रदेश","Bagmati Province"),
                new Province(4,"गण्डकी प्रदेश","Gandaki Province"),
                new Province(5,"लुम्बिनी प्रदेश","Lumbini Province"),
                new Province(6,"कर्णाली प्रदेश","Karnali Province"),
                new Province(7,"सुदूरपश्चिम प्रदेश","Sudurpashchim Province"),
            };

                await _context.Provinces.AddRangeAsync(provinces);
                await _unitOfWork.SaveChangesAsync();
            }
        }
        #endregion

    }
}
