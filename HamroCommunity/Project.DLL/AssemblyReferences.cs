using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Project.DLL.DbContext;
using Project.DLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.DLL
{
    public static class AssemblyReferences
    {
        public static IServiceCollection AddDAL(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), 
            x=>x.MigrationsAssembly("Project.DAL"))
            );

            services.AddIdentity<ApplicationUsers, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();


            services.AddAuthentication(JwtBearerDefaults)

            return services;

        }
    }
}
