using AutoMapper;
using HamroCommunity.Configs;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project.BLL.DTOs.Branch;
using Project.BLL.DTOs.Department;
using Project.BLL.Services.Implementation;
using Project.BLL.Services.Interface;
using Project.DLL.Models;
using System.Text.Json;

namespace HamroCommunity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAllOrigins")]
    public class DepartmentController : HamroCommunityBaseController
    {
        private readonly IDepartmentRepository _departmentRepository;

        public DepartmentController(IDepartmentRepository departmentRepository, IMapper mapper, UserManager<ApplicationUsers> userManager, RoleManager<IdentityRole> roleManager) : base(mapper, userManager, roleManager)
        {
            _departmentRepository  = departmentRepository;
        }

        [HttpPost("SaveDepartment")]
        public async Task<IActionResult> SaveDepartment([FromBody] DepartmentCreateDTOs departmentCreateDTOs)
        {
            var saveDepartmentResult = await _departmentRepository.SaveDepartment(departmentCreateDTOs);
            #region switch
            return saveDepartmentResult switch
            {
                { IsSuccess: true, Data: not null } => CreatedAtAction(nameof(SaveDepartment), saveDepartmentResult.Data),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(saveDepartmentResult.Errors),
                _ => BadRequest("Invalid Some Fields")
            };
            #endregion
        }

        [HttpGet("DeleteDepartment/{DepartmentId}")]
        public async Task<IActionResult> DeleteBranch([FromRoute] string DepartmentId, CancellationToken cancellationToken)
        {
            var deleteDepartmentResult = await _departmentRepository.DeleteDepartment(DepartmentId, cancellationToken);

            #region switch
            return deleteDepartmentResult switch
            {
                { IsSuccess: true, Data: not null } => NoContent(),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(deleteDepartmentResult.Errors),
                _ => BadRequest("Invalid some Fields")
            };
            #endregion
        }

        [HttpPatch("UpdateDepartment/{DepartmentId}")]
        public async Task<IActionResult> UpdateDepartment([FromRoute] string DepartmentId, [FromBody] DepartmentUpdateDTOs departmentUpdateDTOs)
        {
            var updateDepartmentResult = await _departmentRepository.UpdateDepartment(DepartmentId, departmentUpdateDTOs);

            #region switch
            return updateDepartmentResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(updateDepartmentResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(updateDepartmentResult.Errors),
                _ => BadRequest("Invalid DepartmentId")
            };
            #endregion
        }


        [HttpGet("get-all-departmentData")]
        public async Task<IActionResult> GetAll([FromQuery] int pageIndex, [FromQuery] int PageSize, CancellationToken cancellationToken)
        {
            var getAlldepartmenthData = await _departmentRepository.GetAll(pageIndex, PageSize, cancellationToken);


            #region switch
            return getAlldepartmenthData switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(getAlldepartmenthData.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(getAlldepartmenthData.Errors),
                _ => BadRequest("Invalid Data")
            };
            #endregion

        }

        [HttpGet("get-departmentdata-byId/{DepartmentId}")]
        public async Task<IActionResult> GetById([FromRoute] string DepartmentId, CancellationToken cancellationToken)
        {
            var getByIdResultData = await _departmentRepository.GetById(DepartmentId, cancellationToken);

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
