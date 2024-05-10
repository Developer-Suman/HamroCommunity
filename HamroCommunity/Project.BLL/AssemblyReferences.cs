using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Project.BLL.Abstraction;
using Project.BLL.Repository.Implementation;
using Project.BLL.Repository.Interface;
using Project.BLL.Services.Implementation;
using Project.BLL.Services.Interface;
using Project.DLL.Abstraction;
using Project.DLL.JWT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL
{
    public static class AssemblyReferences
    {
        public static IServiceCollection AddBLL(this IServiceCollection services)
        {
            #region CORS Enable
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins",
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                    });

            });

            #endregion

            #region AutoMapper Configuration
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
            #endregion

            #region InjectDependency
            services.AddAuthorization();
            //builder.Services.AddAuthorization();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IJwtProviders, JwtProviders>();
            services.AddScoped<IAuthenticationRepository, AuthenticationRepository>();
            services.AddScoped<IAccountServices, AccountServices>();
            services.AddMemoryCache();
            services.AddScoped<IMemoryCacheRepository, MemoryCacheRepository>();

            #endregion
            return services;
        }
    }
}
