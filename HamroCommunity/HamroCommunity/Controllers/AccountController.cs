using AutoMapper;
using HamroCommunity.Configs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using Project.BLL.DTOs.Authentication;
using Project.BLL.Services.Interface;
using Project.DLL.Abstraction;
using Project.DLL.Models;
using System.Text.Json;

namespace HamroCommunity.Controllers
{
    //[Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : HamroCommunityBaseController
    {
        private readonly IAccountServices _accountServices;
        private readonly IAuthenticationRepository _authenticationRepository;
        private readonly IJwtProviders _jwtProviders;

        public AccountController(IAccountServices accountServices, IAuthenticationRepository authenticationRepository,IJwtProviders jwtProviders, IMapper mapper, UserManager<ApplicationUsers> userManager, RoleManager<IdentityRole> roleManager) : base(mapper, userManager, roleManager)
        {
            _accountServices = accountServices;
            _authenticationRepository = authenticationRepository;
            _jwtProviders = jwtProviders;      
        }

        #region LogIn
        [HttpPost("Login")]
        public async Task<IActionResult> LogIn([FromBody] LogInDTOs logInDTOs)
        {
            var logInResult = await _accountServices.LoginUser(logInDTOs);
        
            #region Switch Statement
            return logInResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(logInResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(logInResult.Errors),
                _ => BadRequest("Invalid Username and Password")
            };

            #endregion
        }

        #endregion

        #region Registration
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegistrationCreateDTOs registrationCreateDTOs)
        {
            var registrationResult = await _accountServices.RegisterUser(registrationCreateDTOs);

            #region switch Statement
            return registrationResult switch
            {
                { IsSuccess: true, Data: not null } => CreatedAtAction(nameof(Register), registrationResult.Data),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(registrationResult.Errors),
                _ => BadRequest("Invalid Fields for Register User")
            };
            #endregion
        }
        #endregion

        #region Create Roles
        [HttpPost("CreateRole")]
        public async Task<IActionResult> CreateRoles([FromQuery] string rolename)
        {
            var roleResult = await _accountServices.CreateRoles(rolename);
            #region switch Statement
            return roleResult switch
            {
                { IsSuccess: true, Data: not null } => CreatedAtAction(nameof(CreateRoles), roleResult.Data),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(roleResult.Errors),
                _ => BadRequest("Invalid rolename Fields")
            };
            #endregion
        }
        #endregion

        #region Assign Roles
        [HttpPost("AssignRoles")]
        public async Task<IActionResult> AssignRoles(AssignRolesDTOs assignRolesDTOs)
        {
            var assignRolesResult = await _accountServices.AssignRoles(assignRolesDTOs);
            #region switch Statement
            return assignRolesResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(assignRolesResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(assignRolesResult.Errors),
                _ => BadRequest("Invalid rolename and userId Fields ")
            };
            #endregion
        }
        #endregion

        #region New RefreshToken
        [HttpPost("GetNewToken")]
        public async Task<IActionResult> GetNewToken([FromBody] TokenDTOs tokenDTOs)
        {
            var getNewTokenResult = await _accountServices.GetNewToken(tokenDTOs);

            #region Switch Statement
            return getNewTokenResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(getNewTokenResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(getNewTokenResult.Errors),
                _ => BadRequest("Invalid accesstoken and refreshtoken Fields ")
            };
            #endregion
        }
        #endregion


        #region GetAllUser
        [HttpGet("GetAllUser")]
        public async Task<IActionResult> GetAllUser(int page, int pageSize, CancellationToken cancellationToken)
        {

            var userRoles = GetCurrentUserRoles();
            var getAllUserResult =await _accountServices.GetAllUsers(page, pageSize, cancellationToken);
            #region Switch Statement
            return getAllUserResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(getAllUserResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(getAllUserResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields ")
            };
            #endregion
        }
        #endregion


        #region GetByUserId
        [HttpGet("GetByUserId")]
        public async Task<IActionResult> GetByUserId(string Id, CancellationToken cancellationToken)
        {
            var getbyUserIdResult = await _accountServices.GetByUserId(Id, cancellationToken);

            #region Switch Statement
            return getbyUserIdResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(getbyUserIdResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(getbyUserIdResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields ")
            };
            #endregion
        }
        #endregion

        #region GetAllRoles
        [HttpGet("GetAllRoles")]
        public async Task<IActionResult> GetAllUserRoles(int page, int pageSize, CancellationToken cancellationToken)
        {
            var getAllUserRolesResult = await _accountServices.GetAllRoles(page, pageSize, cancellationToken);
            #region Switch Statement
            return getAllUserRolesResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(getAllUserRolesResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(getAllUserRolesResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields ")
            };
            #endregion

        }
        #endregion

        #region LogOutUser
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("LogOut")]
        public async Task<IActionResult> logOut()
        {
            await GetCurrentUser();
            var LogOutResult = await _accountServices.LogoutUser(_currentUser!.Id.ToString());
            #region Switch Statement
            return LogOutResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(LogOutResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(LogOutResult.Errors),
                _ => BadRequest("Invalid page and pageSize Fields ")
            };
            #endregion
        }

        #endregion
    }
}
