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
        private readonly IMapper _mapper;
        private readonly IMemoryCacheRepository _memoryCacheRepository;

        public LocationController(IMemoryCacheRepository memoryCacheRepository,IProvinceRepository provinceRepository, IMapper mapper, UserManager<ApplicationUsers> userManager, RoleManager<IdentityRole> roleManager) : base(mapper,userManager, roleManager)
        {

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
    }
}
