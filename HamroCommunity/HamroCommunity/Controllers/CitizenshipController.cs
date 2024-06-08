using AutoMapper;
using HamroCommunity.Configs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project.BLL.DTOs.Citizenship;
using Project.BLL.DTOs.DocumentsDTOs;
using Project.BLL.Services.Implementation;
using Project.BLL.Services.Interface;
using Project.DLL.Models;
using System.Text.Json;

namespace HamroCommunity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CitizenshipController : HamroCommunityBaseController
    {
        private readonly ICitizenshipRepository _citizenshipRepository;

        public CitizenshipController(ICitizenshipRepository citizenshipRepository,IMapper mapper,UserManager<ApplicationUsers> userManager, RoleManager<IdentityRole> roleManager): base(mapper, userManager, roleManager)
        {
            _citizenshipRepository = citizenshipRepository;
            
        }

        [HttpPost("SaveCitizenship")]
        public async Task<IActionResult> SaveCitizenship([FromForm] CitizenshipCreateDTOs citizenshipCreateDTOs, List<CitizenshipImagesDTOs> citizenshipImages)
        {
            var saveCitizenshipResult = await _citizenshipRepository.SaveCitizenshipData(citizenshipCreateDTOs, citizenshipImages);
            #region switch
            return saveCitizenshipResult switch
            {
                { IsSuccess: true, Data: not null } => CreatedAtAction(nameof(SaveCitizenship), saveCitizenshipResult.Data),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(saveCitizenshipResult.Errors),
                _ => BadRequest("Invalid Some Fields")
            };
            #endregion
        }

        [HttpGet("DeleteCitizenship/{CitizenshipId}")]
        public async Task<IActionResult> DeleteDocuments([FromRoute] string CitizenshipId)
        {
            var deleteCitizenshipResult = await _citizenshipRepository.DeleteCitizenshipData(CitizenshipId);

            #region switch
            return deleteCitizenshipResult switch
            {
                { IsSuccess: true, Data: not null } => NoContent(),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(deleteCitizenshipResult.Errors),
                _ => BadRequest("Invalid some Fields")
            };
            #endregion
        }

        [HttpPatch("UpdateCitizenship/{CitizenshipId}")]
        public async Task<IActionResult> UpdateDocuments([FromRoute] string CitizenshipId, [FromForm] CitizenshipUpdateDTOs citizenshipUpdateDTOs)
        {
            var updateCitizenshipResult = await _citizenshipRepository.UpdateCitizenshipData(CitizenshipId, citizenshipUpdateDTOs);

            #region switch
            return updateCitizenshipResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(updateCitizenshipResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(updateCitizenshipResult.Errors),
                _ => BadRequest("Invalid SignitureId")
            };
            #endregion
        }


        [HttpGet("get-all-citizenshipData")]
        public async Task<IActionResult> GetAll([FromQuery] int pageIndex, [FromQuery] int PageSize, CancellationToken cancellationToken)
        {
            var getAllCitizenshipData = await _citizenshipRepository.GetAllCitizenshipData(pageIndex, PageSize, cancellationToken);


            #region switch
            return getAllCitizenshipData switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(getAllCitizenshipData.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(getAllCitizenshipData.Errors),
                _ => BadRequest("Invalid SignitureId")
            };
            #endregion

        }

        [HttpGet("get-citizenshipdata-byId/{CitizenshipId}")]
        public async Task<IActionResult> GetById([FromRoute] string CitizenshipId, CancellationToken cancellationToken)
        {
            var getByIdResultData = await _citizenshipRepository.GetCitizenshipDataById(CitizenshipId, cancellationToken);

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
