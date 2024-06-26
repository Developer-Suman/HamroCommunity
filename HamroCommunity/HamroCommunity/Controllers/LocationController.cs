﻿using AutoMapper;
using HamroCommunity.Configs;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project.BLL.Services.Interface;
using Project.DLL.Models;
using System.Text.Json;

namespace HamroCommunity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAllOrigins")]
    //[EnableCors("AllowAllOrigins")]
    public class LocationController : HamroCommunityBaseController
    {

        private readonly IProvinceRepository _provinceRepository;
        private readonly IDistrictRepository _districtRepository;
        private readonly IMunicipalityRepository _municipalityRepository;
        private readonly IVDCRepository _vdcRepository;
        private readonly IMapper _mapper;
        private readonly IMemoryCacheRepository _memoryCacheRepository;

        public LocationController(IMemoryCacheRepository memoryCacheRepository,IVDCRepository vDCRepository,IMunicipalityRepository municipalityRepository,IDistrictRepository districtRepository,IProvinceRepository provinceRepository, IMapper mapper, UserManager<ApplicationUsers> userManager, RoleManager<IdentityRole> roleManager) : base(mapper,userManager, roleManager)
        {
            _municipalityRepository = municipalityRepository;
            _vdcRepository = vDCRepository;
            _districtRepository = districtRepository;
            _memoryCacheRepository = memoryCacheRepository;
            _mapper = mapper;
            _provinceRepository = provinceRepository;

        }

        [HttpGet("Province/get-all")]
        public async Task<IActionResult> GetAll()
        {
            var getallProvinceData = await _provinceRepository.GetAll();
            #region switch
            return getallProvinceData switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(getallProvinceData.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: false, Errors: not null}=> HandleFailureResult(getallProvinceData.Errors),
                _ => BadRequest("Invalid some Fields")
            };

            #endregion
        }

        [HttpGet("Province/get-by-id")]
        public async Task<IActionResult> GetById(int Id)
        {
        
            var getByIdProvinceData = await _provinceRepository.GetById(Id);

            #region switch
            return getByIdProvinceData switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(getByIdProvinceData.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(getByIdProvinceData.Errors),
                _ => BadRequest("Invalid Some Fields")
            };

            #endregion
        }

        [HttpGet("District/get-by-id")]
        public async Task<IActionResult> GetDistrictById(int Id)
        {
            var getByIdDistrictData = await _districtRepository.GetById(Id);
            #region Switch
            return getByIdDistrictData switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(getByIdDistrictData.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(getByIdDistrictData.Errors),
                _ => BadRequest(" invalid some Fields")
            };
            #endregion
        }

        [HttpGet("District/get-all")]
        public async Task<IActionResult> GetAllDistrict()
        {
            var getDistrictData = await _districtRepository.GetAll();

            #region Switch
            return getDistrictData switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(getDistrictData.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(getDistrictData.Errors),
                _ => BadRequest("Invalid some Fields")
            };
            #endregion
        }

        [HttpGet("District/get-districtby-provinceId")]
        public async Task<IActionResult> GetDistrictByProvinceId(int ProvinceId)
        {
            var getDistrictData = await _districtRepository.GetByProvinceId(ProvinceId);

            #region Switch
            return getDistrictData switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(getDistrictData.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess : false, Errors: not null } => HandleFailureResult(getDistrictData.Errors),
                _ => BadRequest("Invalid some Field")
            };;

            #endregion
        }


        [HttpGet("Municipality/get-municipalby-districtId")]
        public async Task<IActionResult> GetMunicipalityByDistrictId(int DistrictId)
        {
            var getMunicipalData = await _municipalityRepository.GetByDistrictId(DistrictId);

            #region Switch
            return getMunicipalData switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(getMunicipalData.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(getMunicipalData.Errors),
                _ => BadRequest("Invalid some Field")
            };

            #endregion
        }


        [HttpGet("Municipality/get-by-Id")]
        public async Task<IActionResult> GetMunicipalityBytId(int MunicipalityId)
        {
            var getMunicipalData = await _municipalityRepository.GetById(MunicipalityId);

            #region Switch
            return getMunicipalData switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(getMunicipalData.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(getMunicipalData.Errors),
                _ => BadRequest("Invalid some Field")
            };

            #endregion
        }


        [HttpGet("Municipality/get-all")]
        public async Task<IActionResult> GetAllMunicipality()
        {
            var getMunicipalData = await _municipalityRepository.GetAll();

            #region Switch
            return getMunicipalData switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(getMunicipalData.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(getMunicipalData.Errors),
                _ => BadRequest("Invalid some Field")
            };

            #endregion
        }

        [HttpGet("VDC/get-vdcBy-districtId")]
        public async Task<IActionResult> GetVDCByDistrictId(int DistrictId)
        {
            var getVDCData = await _vdcRepository.GetByDistrictId(DistrictId);

            #region Switch
            return getVDCData switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(getVDCData.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(getVDCData.Errors),
                _ => BadRequest("Invalid some Field")
            };

            #endregion
        }



        [HttpGet("VDC/get-all")]
        public async Task<IActionResult> GetAllVDC()
        {
            var getVDCData = await _vdcRepository.GetAll();

            #region Switch
            return getVDCData switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(getVDCData.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(getVDCData.Errors),
                _ => BadRequest("Invalid some Field")
            };

            #endregion
        }



        [HttpGet("VDC/get-by-Id")]
        public async Task<IActionResult> GetVDCBytId(int vdcId)
        {
            var getVDCData = await _vdcRepository.GetById(vdcId);

            #region Switch
            return getVDCData switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(getVDCData.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(getVDCData.Errors),
                _ => BadRequest("Invalid some Field")
            };

            #endregion
        }
    }
}
