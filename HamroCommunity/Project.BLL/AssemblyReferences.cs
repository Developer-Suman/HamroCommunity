using AspNetCoreRateLimit;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Project.BLL.Abstraction;
using Project.BLL.Repository.Implementation;
using Project.BLL.Services.Implementation;
using Project.BLL.Services.Interface;
using Project.DLL.Abstraction;
using Project.DLL.JWT;
using Project.DLL.RepoInterface;
using Project.DLL.Seed;
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
                        builder.WithOrigins("")
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
            services.AddControllers();
            services.AddAuthorization();
            //builder.Services.AddAuthorization();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IJwtProviders, JwtProviders>();
            services.AddScoped<IAuthenticationRepository, AuthenticationRepository>();
            services.AddScoped<IAccountServices, AccountServices>();
            services.AddMemoryCache();
            services.AddScoped<IMemoryCacheRepository, MemoryCacheRepository>();
            services.AddTransient<DataSeeder>();
            services.AddScoped<INashuRepository, NashuRepository>();
            services.AddTransient<IimageRepository, ImageRepository>();
            services.AddTransient<IHelpherMethods, HelpherMethod>();
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
            services.AddScoped<IProvinceRepository, ProvinceRepository>();
            services.AddTransient<IDistrictRepository, DistrictRepository>();
            services.AddTransient<IMunicipalityRepository, MunicipalityRepository>();
            services.AddTransient<IVDCRepository, VDCRepository>();
            services.AddScoped<ISignitureRepository, SignitureRepository>();
            services.AddScoped<IDocumentsRepository, DocumentRepository>();
            services.AddScoped<ICertificateRepository, CertificateRepository>();
            services.AddScoped<ICitizenshipRepository, CitizenshipRepository>();
            services.AddScoped<IBranchRepository, BranchRepository>();
            services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            services.AddScoped<INijamatiRepository, NijamatiRepository>();
            services.AddScoped<IUserDataRepository, UserDataRepository>();





            #endregion
            return services;
        }
    }
}
