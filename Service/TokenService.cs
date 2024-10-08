﻿
using Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Service.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Service
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly SymmetricSecurityKey _symmetricSecurityKey;
        private readonly ISubscriptionService _subscriptionService;

        public TokenService(IConfiguration configuration, ISubscriptionService subscriptionService)
        {
            _subscriptionService = subscriptionService;
            _configuration = configuration;
            _symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
        }

        public string GetToken(AppUser user, IList<string> roles)
        {
            List<Claim> claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.NameId,user.Id),
                new(JwtRegisteredClaimNames.Email,user.Email),
                new(ClaimTypes.GivenName,user.Name),
                new(ClaimTypes.Surname,user.Surname),
                new("SubscriptionStatus", _subscriptionService.GetByUserId(user.Id).Result.SubscriptionType),
            };

            claims.AddRange(roles.Select(m => new Claim(ClaimTypes.Role, m)));
            
            SigningCredentials credentials = new(_symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            
            SecurityTokenDescriptor tokenDescriptor = new()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = credentials,
                Audience = _configuration["Jwt:Audience"],
                Issuer = _configuration["Jwt:Issuer"]
            };

            JwtSecurityTokenHandler securityTokenHandler = new();

            var token = securityTokenHandler.CreateToken(tokenDescriptor);

            return securityTokenHandler.WriteToken(token);
        }
    }
}
