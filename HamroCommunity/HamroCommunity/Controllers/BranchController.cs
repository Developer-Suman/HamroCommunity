﻿using AutoMapper;
using HamroCommunity.Configs;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project.BLL.DTOs.Branch;
using Project.BLL.DTOs.CertificateDTOs;
using Project.BLL.Services.Implementation;
using Project.BLL.Services.Interface;
using Project.DLL.Models;
using System.Text.Json;

namespace HamroCommunity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAllOrigins")]
    public class BranchController : HamroCommunityBaseController
    {
        private readonly IBranchRepository _branchRepository;

        public BranchController(IBranchRepository branchRepository, IMapper mapper, UserManager<ApplicationUsers> userManager, RoleManager<IdentityRole> roleManager) : base(mapper, userManager, roleManager)
        {
            _branchRepository = branchRepository;
            
        }

        [HttpPost]
        public async Task<IActionResult> Save([FromBody] BranchCreateDTOs branchCreateDTOs)
        {
            var saveBranchResult = await _branchRepository.SaveBranch(branchCreateDTOs);
            #region switch
            return saveBranchResult switch
            {
                { IsSuccess: true, Data: not null } => CreatedAtAction(nameof(Save), saveBranchResult.Data),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(saveBranchResult.Errors),
                _ => BadRequest("Invalid Some Fields")
            };
            #endregion
        }

        [HttpDelete("{BranchId}")]
        public async Task<IActionResult> Delete([FromRoute] string BranchId, CancellationToken cancellationToken)
        {
            var deleteBranchResult = await _branchRepository.DeleteBranch(BranchId, cancellationToken);

            #region switch
            return deleteBranchResult switch
            {
                { IsSuccess: true, Data: not null } => NoContent(),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(deleteBranchResult.Errors),
                _ => BadRequest("Invalid some Fields")
            };
            #endregion
        }

        [HttpPatch("{BranchId}")]
        public async Task<IActionResult> Update([FromRoute] string BranchId, [FromBody] BranchUpdateDTOs branchUpdateDTOs)
        {
            var updateBranchResult = await _branchRepository.UpdateBranch(BranchId, branchUpdateDTOs);

            #region switch
            return updateBranchResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(updateBranchResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(updateBranchResult.Errors),
                _ => BadRequest("Invalid SignitureId")
            };
            #endregion
        }


        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int pageIndex, [FromQuery] int PageSize, CancellationToken cancellationToken)
        {
            var getAllbranchData = await _branchRepository.GetAll(pageIndex, PageSize, cancellationToken);


            #region switch
            return getAllbranchData switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(getAllbranchData.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(getAllbranchData.Errors),
                _ => BadRequest("Invalid Data")
            };
            #endregion

        }

        [HttpGet("{BranchId}")]
        public async Task<IActionResult> GetById([FromRoute] string BranchId, CancellationToken cancellationToken)
        {
            var getByIdResultData = await _branchRepository.GetById(BranchId, cancellationToken);

            #region switch
            return getByIdResultData switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(getByIdResultData.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(getByIdResultData.Errors),
                _ => BadRequest("Invalid Data")
            };
            #endregion
        }
    }
}
