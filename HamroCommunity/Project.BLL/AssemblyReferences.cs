using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Project.BLL.Abstraction;
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
            return services;
        }
    }
}
