﻿using AutoMapper;
using HamroCommunity.Configs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using Project.BLL.DTOs.Authentication;
using Project.BLL.Services.Interface;
using Project.DLL.Abstraction;
using System.Text.Json;

namespace HamroCommunity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : HamroCommunityBaseController
    {
        private readonly IAccountServices _accountServices;
        private readonly IAuthenticationRepository _authenticationRepository;
        private readonly IMapper _mapper;
        private readonly IJwtProviders _jwtProviders;

        public AccountController(IAccountServices accountServices, IAuthenticationRepository authenticationRepository, IMapper mapper, IJwtProviders jwtProviders)
        {
            _accountServices = accountServices;
            _authenticationRepository = authenticationRepository;
            _mapper = mapper;
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
        public async Task<IActionResult> Register(RegistrationCreateDTOs registrationCreateDTOs)
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
        public async Task<IActionResult> CreateRoles(string rolename)
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
        public async Task<IActionResult> GetNewToken(TokenDTOs tokenDTOs)
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
    }
}
