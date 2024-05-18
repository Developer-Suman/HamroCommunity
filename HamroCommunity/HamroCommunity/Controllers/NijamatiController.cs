using AutoMapper;
using HamroCommunity.Configs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project.BLL.DTOs.Nashu;
using Project.BLL.Services.Interface;
using Project.DLL.Models;
using System.Text.Json;

namespace HamroCommunity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NijamatiController : HamroCommunityBaseController
    {
        private readonly INashuRepository _nashuRepository;
        private readonly IMapper _mapper;
        private readonly IMemoryCacheRepository _memoryCacheRepository;

        public NijamatiController(IMemoryCacheRepository memoryCacheRepository, IMapper mapper, INashuRepository nashuRepository, UserManager<ApplicationUsers> userManager, RoleManager<IdentityRole> roleManager) : base(mapper, userManager, roleManager)
        {
            _memoryCacheRepository = memoryCacheRepository;
            _mapper = mapper;
            _nashuRepository = nashuRepository;

        }
        [HttpPost("SaveNashu")]
        public async Task<IActionResult> SaveNashu([FromForm] NashuCreateDTOs nashuCreateDTOs)
        {
            var saveNashuResult = await _nashuRepository.SaveNashuData(nashuCreateDTOs);

            #region Switch
            return saveNashuResult switch
            {
                { IsSuccess: true, Data: not null} => CreatedAtAction(nameof(SaveNashu), saveNashuResult.Data),
                { IsSuccess: false, Errors: not null} => HandleFailureResult(saveNashuResult.Errors),
                _ => BadRequest("Invalid Some Fields")
            };
            #endregion

        }

        [HttpGet("DeleteNashu/{NashuId}")]
        public async Task<IActionResult> DeleteNashu([FromRoute] string NashuId)
        {
            var deleteNashuResult = await _nashuRepository.DeleteNashuData(NashuId);

            #region switch
            return deleteNashuResult switch
            {
                { IsSuccess: true, Data: not null } => NoContent(),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(deleteNashuResult.Errors),
                _ => BadRequest("Invalid NashuId")
            };
            #endregion

        }

        [HttpPatch("UpdateNashu/{NashuId}")]
        public async Task<IActionResult> UpdateNashu([FromRoute] string NashuId, [FromBody] NashuUpdateDTOs updateDTOs)
        {
            var updateNashuResult = await _nashuRepository.UpdateNashuData(NashuId,updateDTOs);

            #region switch
            return updateNashuResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(updateNashuResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull

                }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(updateNashuResult.Errors),
                _ => BadRequest("Invalid Nashu Id")
            };

            #endregion
        }


        [HttpGet("get-all-nashudata")]
        public async Task<IActionResult> GetAllData(int page,int pageSize, CancellationToken cancellationToken)
        {
            var getAllNashuData = await _nashuRepository.GetAllNashuData(page, pageSize, cancellationToken);

            #region switch
            return getAllNashuData switch
            {
                { IsSuccess: true, Data: not null} => new JsonResult(getAllNashuData.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: false, Errors: not null} => HandleFailureResult(getAllNashuData.Errors),
                _ => BadRequest("Invalid some Fields")
            };

            #endregion
        }

        [HttpGet("get-nashudata-byId/{NashuId}")]
        public async Task<IActionResult> GetNashuDataById(string NashuId, CancellationToken cancellationToken)
        {
            var getbyIdResult = await _nashuRepository.GetNashuDataById(NashuId, cancellationToken);

            #region switch
            return getbyIdResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(getbyIdResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(getbyIdResult.Errors),
                _ => BadRequest("Invalid NashuId")
            };

            #endregion
        }
    }
}
