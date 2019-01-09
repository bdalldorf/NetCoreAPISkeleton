using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using bdNetCoreAPIDataTransfer;
using bdNetCoreAPIDataTransfer.Requests.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace bdNetCoreAPI.Controllers
{
    [Route("")]
    [AllowAnonymous]
    [ApiController]
    public class Authentication : Controller
    {
        private IConfiguration _Configuration;

        public Authentication(IConfiguration Configuration)
        {
            _Configuration = Configuration;
        }

        [Route(ApiRoutes.ApiAuthenticationRoute.PostWebLoginInformation)]
        [HttpPost]
        public async Task<IActionResult> RequestCookie([FromBody]TokenRequest request)
        {
            if (request.UserName == "Test" && request.Password == "Password")
            {
                var claims = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, request.UserName)
                }, CookieAuthenticationDefaults.AuthenticationScheme);

                var userIdentity = new ClaimsPrincipal(claims);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userIdentity);

                return Redirect("/lessons");
            }

            return BadRequest("Could not verify username and password");
        }

        [Route(ApiRoutes.ApiAuthenticationRoute.PostApiLoginInformation)]
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

                //TODO: Add identity information to the fingerprint (i.e. server info, ect.)
                var fingerPrint = Guid.NewGuid();

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    fingerPrint
                });
            }

            return BadRequest("Could not verify username and password");
        }
    }
}
