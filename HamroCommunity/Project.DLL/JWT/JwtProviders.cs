using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Project.DLL.Abstraction;
using Project.DLL.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Project.DLL.JWT
{
    public class JwtProviders : IJwtProviders
    {
        private readonly IConfiguration _configuration;

        public JwtProviders(IConfiguration configuration)
        {
            _configuration = configuration;
            
        }
        public string Generate(ApplicationUsers users, IList<string> roles)
        {
            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Sub, users.Id),
                new Claim(ClaimTypes.Name, users.UserName!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())

            };

            if(roles is not null && roles.Count > 0 )
            {
                foreach( var role in roles )
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }
            }

            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!)),
                SecurityAlgorithms.HmacSha256
                );

            _ = int.TryParse(_configuration["Jwt:TokenValidityInMinutes"], out int tokenValidityInMinutes);

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                null,
                DateTime.Now.AddMinutes(tokenValidityInMinutes),
                signingCredentials
                );

            string tokenValue = new JwtSecurityTokenHandler().WriteToken(token);
            return tokenValue;
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber); 
            return Convert.ToBase64String(randomNumber);
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!)),
                ValidateLifetime = false
            };

            var tokenHandler = new  JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

            if(securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid Token");
            }

            return principal;
        }
    }
}
