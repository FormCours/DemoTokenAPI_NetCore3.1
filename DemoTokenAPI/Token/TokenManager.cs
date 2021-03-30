using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace DemoTokenAPI.Token
{
    public class TokenManager
    {
        private IConfiguration _config;

        public TokenManager(IConfiguration config)
        {
            _config = config;
        }

        public TokenResult GenerateJwtToken(TokenData data)
        {
            DateTime expirationDate = DateTime.Now.AddMinutes(double.Parse(_config["Jwt:LifeTime"]));

            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512);

            Claim[] claims = new[] {
                new Claim(ClaimTypes.Name, data.Username),
                new Claim(ClaimTypes.Role, data.Role.ToString()),
            };

            JwtSecurityToken token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: expirationDate,
                signingCredentials: credentials);

            return new TokenResult(new JwtSecurityTokenHandler().WriteToken(token), expirationDate);
        }


        public TokenData GetDataToken(ClaimsPrincipal currentUser)
        {
            if (currentUser == null) 
                throw new ArgumentNullException();

            if (!currentUser.HasClaim(c => c.Type == ClaimTypes.Name) && !currentUser.HasClaim(c => c.Type == ClaimTypes.Role))
                throw new SecurityTokenException("Missing Claim");

            return new TokenData()
            {
                Username = ExtractClaim(currentUser, ClaimTypes.Name),
                Role = (TokenData.RoleEnum)Enum.Parse(typeof(TokenData.RoleEnum), ExtractClaim(currentUser, ClaimTypes.Role))
            };
        }
        private string ExtractClaim(ClaimsPrincipal currentUser, string typeClaim)
        {
            return currentUser.Claims.SingleOrDefault(c => c.Type == typeClaim).Value;
        }
    }
}
