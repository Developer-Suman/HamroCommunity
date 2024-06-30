using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Project.BLL.DTOs.Authentication;
using Project.BLL.DTOs.Branch;
using Project.BLL.DTOs.CertificateDTOs;
using Project.BLL.DTOs.Citizenship;
using Project.BLL.DTOs.Department;
using Project.BLL.DTOs.District;
using Project.BLL.DTOs.DocumentsDTOs;
using Project.BLL.DTOs.Municipality;
using Project.BLL.DTOs.Nashu;
using Project.BLL.DTOs.Nijamati;
using Project.BLL.DTOs.Pagination;
using Project.BLL.DTOs.Province;
using Project.BLL.DTOs.Signiture;
using Project.BLL.DTOs.UserData;
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
            .ConstructUsing(src => new SignitureGetDTOs(src.Id,src.SignatureURL, src.CreatedAt))
            .ReverseMap();

            CreateMap<CertificateImages,CertificateGetDTOs>()
                .ForMember(dest => dest.certificateImages, opt => opt.MapFrom(src => src.Certificate.CertificateImages));

            CreateMap<Documents, DocumentsGetDTOs>()
            .ForMember(dest => dest.certificateIds, opt => opt.MapFrom(src => src.certificateDocuments.Select(cd => cd.CertificateId).ToList()));


            
          


         
            CreateMap<PagedResult<Citizenship>, PagedResult<CitizenshipGetDTOs>>().ReverseMap();



            #region certificate
            CreateMap<PagedResult<Certificate>, PagedResult<CertificateGetDTOs>>().ReverseMap();
            CreateMap<PagedResult<Certificate>, PagedResult<CertificateGetDTOs>>()
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items))
            .ReverseMap()
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));

            CreateMap<Certificate, CertificateGetDTOs>()
           .ForMember(dest => dest.certificateImages, opt => opt.MapFrom(src => src.CertificateImages))
           .ReverseMap();
            CreateMap<CertificateCreateDTOs, Certificate>().ReverseMap();
            CreateMap<Certificate, CertificateUpdateDTOs>().ReverseMap();

            #endregion




            #region Branch Mapping
            CreateMap<BranchGetDTOs, Branch>().ReverseMap();
            CreateMap<BranchUpdateDTOs, Branch>().ReverseMap();
            CreateMap<PagedResult<Branch>, PagedResult<BranchGetDTOs>>().ReverseMap();
            #endregion

            #region Department Mapping
            CreateMap<DepartmentGetDTOs, Department>().ReverseMap();
            CreateMap<DepartmentUpdateDTOs, Department>().ReverseMap();
            CreateMap<PagedResult<Department>, PagedResult<DepartmentGetDTOs>>().ReverseMap();
            #endregion

            #region Nijamati Mapping
            CreateMap<NijamatiCreateDTOs, Nijamati>().ReverseMap();
            CreateMap<NijamatiUpdateDTOs, Nijamati>().ReverseMap();
            CreateMap<NijamatiGetDTOs, Nijamati>().ReverseMap();
            CreateMap<PagedResult<Nijamati>, PagedResult<NijamatiGetDTOs>>().ReverseMap();
            #endregion

            #region UserData Mapping
            CreateMap<PagedResult<UserData>, PagedResult<GetUserDataDTOs>>().ReverseMap();
            CreateMap<PagedResult<UserData>, PagedResult<GetUserDataDTOs>>()
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items))
            .ReverseMap()
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));

            CreateMap<UserData, GetUserDataDTOs>()
           .ForMember(dest => dest.imageURLs, opt => opt.MapFrom(src => src.ImageURL))
           .ReverseMap();
            CreateMap<CreateUserDataDTOs, UserData>().ReverseMap();
            CreateMap<UserData, UpdateUserDataDTOs>().ReverseMap();

            #endregion


            




        }
    }
}
