using System;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Configuration;
using System.Text;

namespace WebAPI.Helpers
{
    public class JwtHelper
    {
        public static dynamic ReadToken(string token, string property = null){
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var claims = tokenHandler.ReadJwtToken(token).Claims.Select(
                    c => new { c.Type, c.Value }
                );
                return property == null ? claims : (dynamic)claims.Where(c => c.Type == property).SingleOrDefault().Value;
            }
            catch { throw new ArgumentException("Invalid token!"); }
        }
        public static string CreateToken(Claim[] claims, DateTime expire)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(ConfigurationManager.AppSettings["SecretKey"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = expire,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key), 
                    SecurityAlgorithms.HmacSha256Signature
                )
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
