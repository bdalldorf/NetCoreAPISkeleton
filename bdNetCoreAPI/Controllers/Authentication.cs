using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using bdNetCoreAPIDataTransfer;
using bdNetCoreAPIDataTransfer.Requests.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace bdNetCoreAPI.Controllers
{
    [Route("")]
    [ApiController]
    public class Authentication : Controller
    {
        private IConfiguration _Configuration;

        public Authentication(IConfiguration Configuration)
        {
            _Configuration = Configuration;
        }

        [AllowAnonymous]
        [Route(ApiRoutes.ApiAuthenticationRoute.PostLoginInformation)]
        [HttpPost]
        public IActionResult RequestToken([FromBody]TokenRequest request)
        {
            if (request.UserName == "Test" && request.Password == "Password")
            {
                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, request.UserName)
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_Configuration["SecurityKey"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: ApiConstants.ValidIssuer,
                    audience: ApiConstants.ValidAudience,
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: creds);

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token)
                });
            }

            return BadRequest("Could not verify username and password");
        }
    }
}
