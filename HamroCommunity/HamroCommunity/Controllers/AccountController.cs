using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project.BLL.DTOs;
using Project.BLL.Services.Interface;
using Project.DLL.Abstraction;
using System.Text.Json;

namespace HamroCommunity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
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
        public async Task<IActionResult> LogIn([FromBody] LogInDTOs logInDTOs)
        {
            var logInResult = await _accountServices.LoginUser(logInDTOs);
            //if(logInResult.Data is not null)
            //{
            //    return Ok(logInResult.Data);
            //}
            //else
            //{
            //    return Unauthorized(logInResult.Errors);
            //}

            #region Switch Statement
            return logInResult switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(logInResult.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                }),
           
                _ => BadRequest("Invalid Username and Password")
            };;

            #endregion
        }

        #endregion
    }
}
