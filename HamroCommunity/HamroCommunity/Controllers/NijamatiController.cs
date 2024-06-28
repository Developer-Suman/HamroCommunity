using AutoMapper;
using HamroCommunity.Configs;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project.BLL.DTOs.Nashu;
using Project.BLL.DTOs.Nijamati;
using Project.BLL.Services.Interface;
using Project.DLL.Models;
using System.Text.Json; 

namespace HamroCommunity.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("AllowAllOrigins")]
    [ApiController]
    public class NijamatiController : HamroCommunityBaseController
    {
        private readonly INijamatiRepository _nijamatiRepository;
        private readonly IMapper _mapper;
        private readonly IMemoryCacheRepository _memoryCacheRepository;

        public NijamatiController(IMemoryCacheRepository memoryCacheRepository, IMapper mapper, INijamatiRepository nijamatiRepository, UserManager<ApplicationUsers> userManager, RoleManager<IdentityRole> roleManager) : base(mapper, userManager, roleManager)
        {
            _memoryCacheRepository = memoryCacheRepository;
            _mapper = mapper;
            _nijamatiRepository = nijamatiRepository;

        }
        [HttpPost("SaveNijamati")]
        public async Task<IActionResult> SaveNijamati([FromForm] NijamatiCreateDTOs nijamatiCreateDTOs)
        {
            var saveNijamatiResult = await _nijamatiRepository.SaveNijamati(nijamatiCreateDTOs);

            #region Switch
            return saveNijamatiResult switch
            {
                { IsSuccess: true, Data: not null} => CreatedAtAction(nameof(SaveNijamati), saveNijamatiResult.Data),
                { IsSuccess: false, Errors: not null} => HandleFailureResult(saveNijamatiResult.Errors),
                _ => BadRequest("Invalid Some Fields")
            };
            #endregion

        }

        [HttpGet("DeleteNijamati/{NijamatiId}")]
        public async Task<IActionResult> DeleteNijamati([FromRoute] string NijamatiId, CancellationToken cancellationToken)
        {
            var deleteNijamatiResult = await _nijamatiRepository.DeleteNijamati(NijamatiId, cancellationToken);

            #region switch
            return deleteNijamatiResult switch
            {
                { IsSuccess: true, Data: not null } => NoContent(),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(deleteNijamatiResult.Errors),
                _ => BadRequest("Invalid some Fields")
            };
            #endregion

        }

        [HttpPatch("UpdateNijamati/{NijamatiId}")]
        public async Task<IActionResult> UpdateNijamati([FromRoute] string NijamatiId, [FromBody] NijamatiUpdateDTOs updateDTOs)
        {
            var updateNijamatiResult = await _nijamatiRepository.UpdateNijamati(NijamatiId, updateDTOs);

            #region switch
            return updateNijamatiResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(updateNijamatiResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull

                }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(updateNijamatiResult.Errors),
                _ => BadRequest("Invalid some Fields")
            };

            #endregion
        }


        [HttpGet("get-all-nijamatidata")]
        public async Task<IActionResult> GetAllData(int pageIndex, int pageSize, CancellationToken cancellationToken)
        {
            var getAllNijamatiData = await _nijamatiRepository.GetAll(pageIndex, pageSize, cancellationToken);

            #region switch
            return getAllNijamatiData switch
            {
                { IsSuccess: true, Data: not null} => new JsonResult(getAllNijamatiData.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: false, Errors: not null} => HandleFailureResult(getAllNijamatiData.Errors),
                _ => BadRequest("Invalid some Fields")
            };

            #endregion
        }

        [HttpGet("get-nijamatidata-byId/{NijamatiId}")]
        public async Task<IActionResult> GetNashuDataById(string NijamatiId, CancellationToken cancellationToken)
        {
            var getbyIdResult = await _nijamatiRepository.GetById(NijamatiId, cancellationToken);

            #region switch
            return getbyIdResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(getbyIdResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(getbyIdResult.Errors),
                _ => BadRequest("Invalid some Fields")
            };

            #endregion
        }
    }
}
