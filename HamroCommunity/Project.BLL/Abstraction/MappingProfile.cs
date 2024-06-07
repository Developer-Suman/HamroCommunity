using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Project.BLL.DTOs.Authentication;
using Project.BLL.DTOs.District;
using Project.BLL.DTOs.Municipality;
using Project.BLL.DTOs.Nashu;
using Project.BLL.DTOs.Province;
using Project.BLL.DTOs.Signiture;
using Project.BLL.DTOs.Vdc;
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

            CreateMap<ProvinceGetDTOs, Province>().ReverseMap();
            CreateMap<DistrictGetDTOs, District>().ReverseMap();
            CreateMap<MunicipalityGetDTOs, Municipality>().ReverseMap();
            CreateMap<VdcGetDTOs, VDC>().ReverseMap();
            CreateMap<Signature, SignitureCreateDTOs>().ReverseMap();
            CreateMap<Signature, SignitureGetDTOs>().ReverseMap();
            CreateMap<Signature, SignitureUpdateDTOs>().ReverseMap();
            //CreateMap<Province, ProvinceGetDTOs>()
            //.ForCtorParam("ProvinceId", opt => opt.MapFrom(src => src.Id))
            //.ForCtorParam("ProvinceNameInEnglish", opt => opt.MapFrom(src => src.ProvinceNameInEnglish))
            //.ForCtorParam("ProvinceNameInNepali", opt => opt.MapFrom(src => src.ProvinceNameInNepali));

            //CustomMapping using automapper


        }
    }
}
