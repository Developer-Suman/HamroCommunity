using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Project.BLL.DTOs.Authentication;
using Project.BLL.DTOs.CertificateDTOs;
using Project.BLL.DTOs.District;
using Project.BLL.DTOs.DocumentsDTOs;
using Project.BLL.DTOs.Municipality;
using Project.BLL.DTOs.Nashu;
using Project.BLL.DTOs.Pagination;
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
            CreateMap<SignitureCreateDTOs, SignitureGetDTOs>().ReverseMap();
            CreateMap<Signature, SignitureUpdateDTOs>().ReverseMap();
            CreateMap<Documents, DocumentsCreateDTOs>().ReverseMap();
            CreateMap<Documents, DocumentsGetDTOs>().ReverseMap();
            CreateMap<DocumentsCreateDTOs, DocumentsGetDTOs>().ReverseMap();
            CreateMap<Documents, DocumentsUpdateDTOs>().ReverseMap();

            CreateMap<Signature, SignitureGetDTOs>()
            .ConstructUsing(src => new SignitureGetDTOs(src.SignatureId,src.SignatureURL, src.CreatedAt))
            .ReverseMap();

            CreateMap<CertificateImages, CertificateGetDTOs>().ReverseMap();
            CreateMap<CertificateGetDTOs, CertificateImageDTOs>().ReverseMap();


            CreateMap<PagedResult<Certificate>, PagedResult<CertificateGetDTOs>>().ReverseMap();

            CreateMap<PagedResult<Certificate>, PagedResult<CertificateGetDTOs>>()
           .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items))
           .ReverseMap()
           .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));

            CreateMap<Certificate, CertificateGetDTOs>()
           .ForMember(dest => dest.certificateImages, opt => opt.MapFrom(src => src.CertificateImages))
           .ReverseMap()
           .ForMember(dest => dest.CertificateImages, opt => opt.MapFrom(src => src.certificateImages));

           


            CreateMap<Certificate, CertificateUpdateDTOs>() .ReverseMap();

            //CreateMap<Province, ProvinceGetDTOs>()
            //.ForCtorParam("ProvinceId", opt => opt.MapFrom(src => src.Id))
            //.ForCtorParam("ProvinceNameInEnglish", opt => opt.MapFrom(src => src.ProvinceNameInEnglish))
            //.ForCtorParam("ProvinceNameInNepali", opt => opt.MapFrom(src => src.ProvinceNameInNepali));

            //CustomMapping using automapper


        }
    }
}
