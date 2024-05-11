using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Project.BLL.DTOs.Authentication;
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

        }
    }
}
