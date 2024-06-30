using AutoMapper;
using HamroCommunity.Configs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project.BLL.DTOs.Citizenship;
using Project.BLL.DTOs.UserData;
using Project.BLL.Services.Implementation;
using Project.BLL.Services.Interface;
using Project.DLL.Models;
using System.Text.Json;

namespace HamroCommunity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Authorize]
    public class UserDataController : HamroCommunityBaseController
    {
        private readonly IUserDataRepository _userDataRepository;

        public UserDataController(IUserDataRepository userDataRepository,IMapper mapper, UserManager<ApplicationUsers> userManager, RoleManager<IdentityRole> roleManager) : base(mapper, userManager, roleManager)
        {
            _userDataRepository =  userDataRepository;
        }

        [HttpPost("SaveUserData")]
        public async Task<IActionResult> SaveUserData([FromForm] CreateUserDataDTOs createUserDataDTOs, IFormFile imagefiles)
        {
            await GetCurrentUser();
            var saveUserDataResult = await _userDataRepository.SaveUserData(createUserDataDTOs, imagefiles, _currentUser!.Id);
            #region switch
            return saveUserDataResult switch
            {
                { IsSuccess: true, Data: not null } => CreatedAtAction(nameof(SaveUserData), saveUserDataResult.Data),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(saveUserDataResult.Errors),
                _ => BadRequest("Invalid Some Fields")
            };
            #endregion
        }

        [HttpGet("DeleteUserData/{UserDataId}")]
        public async Task<IActionResult> DeleteUserData([FromRoute] string UserDataId)
        {
            var deleteuserDataResult = await _userDataRepository.DeleteUserData(UserDataId);

            #region switch
            return deleteuserDataResult switch
            {
                { IsSuccess: true, Data: not null } => NoContent(),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(deleteuserDataResult.Errors),
                _ => BadRequest("Invalid some Fields")
            };
            #endregion
        }

        [HttpPatch("UpdateuserData/{UserDataId}")]
        public async Task<IActionResult> UpdateDocuments([FromRoute] string UserDataId, [FromForm] UpdateUserDataDTOs userDataDTOs, IFormFile imegeFiles)
        {
            var updateuserDataResult = await _userDataRepository.UpdateUserData(UserDataId, userDataDTOs, imegeFiles);

            #region switch
            return updateuserDataResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(updateuserDataResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(updateuserDataResult.Errors),
                _ => BadRequest("Invalid Id")
            };
            #endregion
        }


        [HttpGet("get-all-userData")]
        public async Task<IActionResult> GetAll([FromQuery] int pageIndex, [FromQuery] int PageSize, CancellationToken cancellationToken)
        {
            var getAluserData = await _userDataRepository.GetAllUserData(pageIndex, PageSize, cancellationToken);


            #region switch
            return getAluserData switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(getAluserData.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(getAluserData.Errors),
                _ => BadRequest("Invalid Id")
            };
            #endregion

        }

        [HttpGet("get-userdata-byId/{UserDataId}")]
        public async Task<IActionResult> GetById([FromRoute] string UserDataId, CancellationToken cancellationToken)
        {
            var getByIdResultData = await _userDataRepository.GetUserDataById(UserDataId, cancellationToken);

            #region switch
            return getByIdResultData switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(getByIdResultData.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(getByIdResultData.Errors),
                _ => BadRequest("Invalid Id")
            };
            #endregion
        }


    }
}
