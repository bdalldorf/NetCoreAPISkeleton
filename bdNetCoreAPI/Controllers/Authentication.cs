using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using bdNetCoreAPIDataTransfer;
using bdNetCoreAPIDataTransfer.Requests.Authentication;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace bdNetCoreAPI.Controllers
{
    [AllowAnonymous]
    [Route("")]
    [ApiController]
    public class Authentication : Controller
    {
        private IConfiguration _Configuration;
        private IAntiforgery _Antiforgery;

        public Authentication(IConfiguration Configuration, IAntiforgery antiforgery)
        {
            _Configuration = Configuration;
            _Antiforgery = antiforgery;
        }

        [HttpPost]
        [Route(ApiRoutes.ApiAuthenticationRoute.PostWebLoginInformation)]
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

                return Ok(new
                {
                    userName = request.UserName,
                    firstName = "Test",
                    lastName = "Tester",
                    fingetPrint = Guid.NewGuid(),
                });
            }

            return BadRequest("Could not verify username and password");
        }

        [Authorize]
        [HttpGet]
        [Route(ApiRoutes.ApiAuthenticationRoute.PostWebLoginAntiForgeryCookie)]
        public IActionResult RequesAntiForgeryCookie()
        {
            var tokens = _Antiforgery.GetAndStoreTokens(HttpContext);
            CookieOptions CookieOptions = new CookieOptions();
            CookieOptions.HttpOnly = false;
            CookieOptions.SameSite = SameSiteMode.Strict;

            HttpContext.Response.Cookies.Append("XSRF-TOKEN", tokens.RequestToken, CookieOptions);

            return Ok();
        }

        [Authorize]
        [HttpPost]
        [Route(ApiRoutes.ApiAuthenticationRoute.PostWebLogoutInformation)]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();

            return Ok(new
            {
                Message = "You have been logged out"
            });
        }

        [HttpPost]
        [Route(ApiRoutes.ApiAuthenticationRoute.PostApiLoginInformation)]
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
