using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.Helpers
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class CustomAuthorize : Attribute, IAuthorizationFilter
    {
        public string[] ExpectMethod;

        private static TokenValidationParameters GetValidationParameters()
        {
            var key = Encoding.ASCII.GetBytes("3a97556f-c07f-4f5a-8bc6-28711d4922e9");

            return new TokenValidationParameters()
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false
            };
        }

        public void OnAuthorization(AuthorizationFilterContext filterContext)
        {
            //cek bearer token
            string authHeader = filterContext.HttpContext.Request.Headers["Authorization"];
            if (string.IsNullOrEmpty(authHeader))
            {
                //cek ExpectMethod
                if (ExpectMethod != null)
                {
                    if (!ExpectMethod.Contains(filterContext.RouteData.Values["action"]))
                    {
                        filterContext.Result = new ChallengeResult();
                    }
                }
                else
                {
                    filterContext.Result = new ChallengeResult();
                }
            }
            else
            {
                try
                {
                    bool skipAuth = false;

                    //cek ExpectMethod
                    if (ExpectMethod != null)
                    {
                        if (!ExpectMethod.Contains(filterContext.RouteData.Values["action"]))
                        {
                            filterContext.Result = new ChallengeResult();
                            skipAuth = true;
                        }
                        skipAuth = false;
                    }

                    if (!skipAuth)
                    {
                        //validate token
                        var tokenHandler = new JwtSecurityTokenHandler();
                        var validationParameters = GetValidationParameters();

                        SecurityToken validatedToken;
                        var principal = tokenHandler.ValidateToken(authHeader, validationParameters, out validatedToken);
                    }
                }
                catch
                {
                    if (ExpectMethod != null)
                    {
                        if (!ExpectMethod.Contains(filterContext.RouteData.Values["action"]))
                        {
                            filterContext.Result = new ChallengeResult();
                        }
                    }
                    else
                    {
                        filterContext.Result = new ChallengeResult();
                    }
                }
            }
        }
    }
}
