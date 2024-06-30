using AutoMapper;
using HamroCommunity.Configs;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project.BLL.DTOs.Signiture;
using Project.BLL.Services.Interface;
using Project.DLL.Models;
using System.Text.Json;

namespace HamroCommunity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAllOrigins")]
    public class SignitureController : HamroCommunityBaseController
    {
        private readonly ISignitureRepository _signitureRepository;


        public SignitureController(ISignitureRepository signitureRepository,IMapper mapper, UserManager<ApplicationUsers> userManager, RoleManager<IdentityRole> roleManager): base(mapper, userManager, roleManager)
        {
            _signitureRepository = signitureRepository;
            
        }

        [HttpPost("SaveSigniture")]
        public async Task<IActionResult> SaveSigniture([FromForm] SignitureCreateDTOs signitureCreateDTOs)
        {
            var saveSignitureResult = await _signitureRepository.SaveSigniture(signitureCreateDTOs);
            #region switch
            return saveSignitureResult switch
            {
                { IsSuccess: true, Data: not null } => CreatedAtAction(nameof(SaveSigniture), saveSignitureResult.Data),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(saveSignitureResult.Errors),
                _ => BadRequest("Invalid Some Fields")
            };
            #endregion
        }

        [HttpGet("DeleteSigniture/{SignitureId}")]
        public async Task<IActionResult> DeleteSigniture([FromRoute] string SignitureId)
        {
            var deleteSignitureResult = await _signitureRepository.DeleteSigniture(SignitureId);

            #region switch
            return deleteSignitureResult switch
            {
                { IsSuccess: true, Data: not null } => NoContent(),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(deleteSignitureResult.Errors),
                _ => BadRequest("Invalid some Fields")
            };
            #endregion
        }

        [HttpPatch("UpdateSigniture/{SignitureId}")]
        public async Task<IActionResult> UpdateSigniture([FromRoute] string SignitureId, [FromForm] SignitureUpdateDTOs signitureUpdateDTOs)
        {
            var updateSignitureResult = await _signitureRepository.UpdateSigniture(SignitureId, signitureUpdateDTOs);

            #region switch
            return updateSignitureResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(updateSignitureResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(updateSignitureResult.Errors),
                _ => BadRequest("Invalid SignitureId")
            };
            #endregion
        }


        [HttpGet("get-all-signitureData")]
        public async Task<IActionResult> GetAll([FromQuery]int pageIndex,[FromQuery] int PageSize, CancellationToken cancellationToken)
        {
            var getAllSignitureData = await _signitureRepository.GetAll(pageIndex, PageSize, cancellationToken);


            #region switch
            return getAllSignitureData switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(getAllSignitureData.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(getAllSignitureData.Errors),
                _ => BadRequest("Invalid SignitureId")
            };
            #endregion

        }

        [HttpGet("get-signituredata-byId/{SignitureId}")]
        public async Task<IActionResult> GetById([FromRoute] string SignitureId, CancellationToken cancellationToken)
        {
            var getByIdResultData = await _signitureRepository.GetById(SignitureId, cancellationToken);

            #region switch
            return getByIdResultData switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(getByIdResultData.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(getByIdResultData.Errors),
                _ => BadRequest("Invalid SignitureId")
            };
            #endregion
        }
    }
}
