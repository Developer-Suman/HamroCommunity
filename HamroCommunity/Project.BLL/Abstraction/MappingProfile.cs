using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Project.BLL.DTOs.Authentication;
using Project.BLL.DTOs.Nashu;
using Project.DLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.Abstraction
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<RegistrationCreateDTOs, ApplicationUsers>().ReverseMap();
            CreateMap<UserDTOs, ApplicationUsers>().ReverseMap();
            CreateMap<UserDTOs, IdentityRole>().ReverseMap();
            CreateMap<ChangePasswordDTOs, IdentityRole>().ReverseMap();
            CreateMap<RoleDTOs, IdentityRole>().ReverseMap();
            CreateMap<NashuCreateDTOs, NashuGetDTOs>().ReverseMap();
            CreateMap<Nashu,NashuCreateDTOs>().ReverseMap();
            CreateMap<Nashu, NashuGetDTOs>().ReverseMap();
            CreateMap<Nashu, NashuUpdateDTOs>().ReverseMap();
            CreateMap<NashuGetDTOs, NashuUpdateDTOs>().ReverseMap();

        }
    }
}
