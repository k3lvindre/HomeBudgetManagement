using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System;
using static HomeBudgetManagement.Api.Core.Controllers.IdentityController;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;

namespace HomeBudgetManagement.Api.Core.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;

        public AuthService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(IdentityUser userAccount)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.UserData, userAccount.UserName),
                new Claim(ClaimTypes.Role,"Admin"),
                new Claim("NickName", "kelv") //used at NickNamePolicy authorization 
            };

            var key = _configuration.GetSection("Jwt:Key").Value;

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

            var signingCred = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);

            var securityToken = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(60),
                issuer: _configuration.GetSection("Jwt:Issuer").Value,
                audience: _configuration.GetSection("Jwt:Audience").Value,
                signingCredentials: signingCred);

            string tokenString = new JwtSecurityTokenHandler().WriteToken(securityToken);
            return tokenString;
        }
    }
}
