using AutoMapper;
using Microsoft.Extensions.Configuration;
using Project.BLL.DTOs;
using Project.BLL.Services.Interface;
using Project.BLL.Validator;
using Project.DLL.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.Services.Implementation
{
    public class AccountServices : IAccountServices
    {
        private readonly IMapper _mapper;
        private readonly IJwtProviders _jwtProviders;
        private readonly IConfiguration _config;
        private readonly IAuthenticationRepository _authenticationRepository;


        public AccountServices(IMapper mapper, IJwtProviders jwtProviders, IConfiguration configuration, IAuthenticationRepository authenticationRepository)
        {
            _mapper = mapper;
            _jwtProviders = jwtProviders;
            _config = configuration;
            _authenticationRepository = authenticationRepository;
            
        }
        public async Task<Result<TokenDTOs>> LoginUser(LogInDTOs logInDTOs)
        {
            try
            {
                #region Validation
                var validationError = LogInValidator.LogInValidate(logInDTOs);
                if(validationError.Any())
                {
                    return Result<TokenDTOs>.Failure(validationError.ToArray());
                }

                #endregion
                var user = await _authenticationRepository.FindByEmailAsync(logInDTOs.Email);
                if(user == null)
                {
                    return Result<TokenDTOs>.Failure("Unauthorized","Invalid Credentials");
                }
                if(!await _authenticationRepository.CheckPasswordAsync(user, logInDTOs.Password))
                {
                    return Result<TokenDTOs>.Failure("Unauthorized","Invalid Password");
                }

                var roles = await _authenticationRepository.GetRolesAsync(user);
                if(roles is null)
                {
                    return Result<TokenDTOs>.Failure("NotFound", "Roles are not found");
                }
                string token = _jwtProviders.Generate(user,roles);

                string refreshToken = _jwtProviders.GenerateRefreshToken();
                user.RefreshToken = refreshToken;

                _ = int.TryParse(_config["Jwt:RefreshTokenValidityInDays"], out int refreshTokenValidityInDays);
                user.RefreshTokenExpiryTime = DateTime.Now.AddDays(refreshTokenValidityInDays);

                await _authenticationRepository.UpdateUserAsync(user);

                var logInData = new TokenDTOs()
                {
                    Token = token,
                    RefreshToken = refreshToken,    
                };

                return Result<TokenDTOs>.Success(logInData);
            }
            catch(Exception ex)
            {
                throw new Exception("Something went Wrong while logging");
            }
        }

        public Task<Result<RegistrationCreateDTOs>> RegisterUser(RegistrationCreateDTOs userModel)
        {
            throw new NotImplementedException();
        }
    }
}
