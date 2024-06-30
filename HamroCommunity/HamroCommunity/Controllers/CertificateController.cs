using AutoMapper;
using HamroCommunity.Configs;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project.BLL.DTOs.CertificateDTOs;
using Project.BLL.Services.Interface;
using Project.DLL.Models;
using System.Collections.Generic;
using System.Text.Json;

namespace HamroCommunity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAllOrigins")]
    public class CertificateController : HamroCommunityBaseController
    {
        private readonly ICertificateRepository _certificateRepository;

        public CertificateController(ICertificateRepository certificateRepository, IMapper mapper, UserManager<ApplicationUsers> userManager,RoleManager<IdentityRole> roleManager): base(mapper,userManager,roleManager)
        {
            _certificateRepository = certificateRepository;
        }

        [HttpPost("SaveCertificate")]
        public async Task<IActionResult> SaveCertificate([FromForm] CertificateCreateDTOs certificateCreateDTOs, List<IFormFile> certificateFiles)
        {
            var saveCertificateResult = await _certificateRepository.SaveCertificate(certificateCreateDTOs, certificateFiles);
            #region switch
            return saveCertificateResult switch
            {
                { IsSuccess: true, Data: not null } => CreatedAtAction(nameof(SaveCertificate), saveCertificateResult.Data),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(saveCertificateResult.Errors),
                _ => BadRequest("Invalid Some Fields")
            };
            #endregion
        }

        [HttpGet("DeleteCertificate/{CertificateId}")]
        public async Task<IActionResult> DeleteCertificate([FromRoute] string CertificateId, CancellationToken cancellationToken)
        {
            var deleteCertificateResult = await _certificateRepository.DeleteCertificate(CertificateId, cancellationToken);

            #region switch
            return deleteCertificateResult switch
            {
                { IsSuccess: true, Data: not null } => NoContent(),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(deleteCertificateResult.Errors),
                _ => BadRequest("Invalid some Fields")
            };
            #endregion
        }

        [HttpPatch("UpdateCertificate/{CertificateId}")]
        public async Task<IActionResult> UpdateCertificate([FromRoute] string CertificateId, [FromForm] CertificateUpdateDTOs certificateUpdateDTOs, List<IFormFile> multipleFiles)
        {
            var updateCertificateResult = await _certificateRepository.UpdateCertificate(CertificateId, certificateUpdateDTOs,multipleFiles);

            #region switch
            return updateCertificateResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(updateCertificateResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(updateCertificateResult.Errors),
                _ => BadRequest("Invalid SignitureId")
            };
            #endregion
        }


        [HttpGet("get-all-certificateData")]
        public async Task<IActionResult> GetAll([FromQuery] int pageIndex, [FromQuery] int PageSize, CancellationToken cancellationToken)
        {
            var getAllCertificateData = await _certificateRepository.GetAll(pageIndex, PageSize, cancellationToken);


            #region switch
            return getAllCertificateData switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(getAllCertificateData.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(getAllCertificateData.Errors),
                _ => BadRequest("Invalid SignitureId")
            };
            #endregion

        }

        [HttpGet("get-certificatedata-byId/{CertificateId}")]
        public async Task<IActionResult> GetById([FromRoute] string CertificateId, CancellationToken cancellationToken)
        {
            var getByIdResultData = await _certificateRepository.GetById(CertificateId, cancellationToken);

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
