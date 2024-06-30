using AutoMapper;
using HamroCommunity.Configs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project.BLL.DTOs.DocumentsDTOs;
using Project.BLL.Services.Interface;
using Project.DLL.Models;
using System.Text.Json;

namespace HamroCommunity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DocumentsController : HamroCommunityBaseController
    {
        private readonly IDocumentsRepository _documentsRepository;

        public DocumentsController(IDocumentsRepository documentsRepository,IMapper mapper, UserManager<ApplicationUsers> userManager, RoleManager<IdentityRole> roleManager): base(mapper,userManager,roleManager)
        {
            _documentsRepository = documentsRepository;
        }

        [HttpPost("SaveDocuments")]
        public async Task<IActionResult> SaveDocuments([FromForm] DocumentsCreateDTOs documentsCreateDTOs)
        {
            await GetCurrentUser();
            var saveDocumentseResult = await _documentsRepository.SaveDocuments(documentsCreateDTOs, _currentUser!.Id);
            #region switch
            return saveDocumentseResult switch
            {
                { IsSuccess: true, Data: not null } => CreatedAtAction(nameof(SaveDocuments), saveDocumentseResult.Data),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(saveDocumentseResult.Errors),
                _ => BadRequest("Invalid Some Fields")
            };
            #endregion
        }

        [HttpGet("DeleteDocuments/{DocumentsId}")]
        public async Task<IActionResult> DeleteDocuments([FromRoute] string DocumentsId)
        {
            var deleteDocumentsResult = await _documentsRepository.DeleteDocuments(DocumentsId);

            #region switch
            return deleteDocumentsResult switch
            {
                { IsSuccess: true, Data: not null } => NoContent(),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(deleteDocumentsResult.Errors),
                _ => BadRequest("Invalid some Fields")
            };
            #endregion
        }

        [HttpPatch("UpdateDocuments/{DocumentsId}")]
        public async Task<IActionResult> UpdateDocuments([FromRoute] string DocumentsId, [FromForm] DocumentsUpdateDTOs documentsUpdateDTOs)
        {
            var updateDocumentsResult = await _documentsRepository.UpdateDocuments(DocumentsId, documentsUpdateDTOs);

            #region switch
            return updateDocumentsResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(updateDocumentsResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(updateDocumentsResult.Errors),
                _ => BadRequest("Invalid SignitureId")
            };
            #endregion
        }


        [HttpGet("get-all-documentsData")]
        public async Task<IActionResult> GetAll([FromQuery] int pageIndex, [FromQuery] int PageSize, CancellationToken cancellationToken)
        {
            var getAllDocumentsData = await _documentsRepository.GetAll(pageIndex, PageSize, cancellationToken);


            #region switch
            return getAllDocumentsData switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(getAllDocumentsData.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(getAllDocumentsData.Errors),
                _ => BadRequest("Invalid SignitureId")
            };
            #endregion

        }

        [HttpGet("get-documentsdata-byId/{DocumentsId}")]
        public async Task<IActionResult> GetById([FromRoute] string DocumentsId, CancellationToken cancellationToken)
        {
            var getByIdResultData = await _documentsRepository.GetById(DocumentsId, cancellationToken);

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
