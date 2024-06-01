using AutoMapper;
using HamroCommunity.Configs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project.BLL.Services.Interface;
using Project.DLL.Models;
using System.Text.Json;

namespace HamroCommunity.Controllers
{
    public class LocationController : HamroCommunityBaseController
    {

        private readonly IProvinceRepository _provinceRepository;
        private readonly IDistrictRepository _districtRepository;
        private readonly IMapper _mapper;
        private readonly IMemoryCacheRepository _memoryCacheRepository;

        public LocationController(IMemoryCacheRepository memoryCacheRepository,IDistrictRepository districtRepository,IProvinceRepository provinceRepository, IMapper mapper, UserManager<ApplicationUsers> userManager, RoleManager<IdentityRole> roleManager) : base(mapper,userManager, roleManager)
        {

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
            string provinceId = Id.ToString();
            var getByIdProvinceData = await _provinceRepository.GetById(provinceId);

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
            string DistrictId = Id.ToString();
            var getByIdDistrictData = await _districtRepository.GetById(DistrictId);
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
            string provinceId = ProvinceId.ToString();
            var getDistrictData = await _districtRepository.GetByProvinceId(provinceId);

            #region Switch
            return getDistrictData switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(getDistrictData.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                _ => BadRequest("Invalid some Field")
            };

            #endregion
        }
    }
}
