using bdNetCoreAPIDataTransfer;
using Microsoft.AspNetCore.Mvc;

namespace bdNetCoreAPI.Controllers
{
    /// <summary>
    /// The HttpGet, HttpPost, HttpPut, HttpDelete Attributes aren't necessary for the methods the way I am using the routes.
    /// But if you added a base route to the class Route Attribute they can be used properly.
    /// </summary>
    [Route("")]
    [ApiController]
    public class APIStartup : Controller
    {
        [Route(ApiRoutes.ApiStartupRoute.GetApiStartup)]
        [HttpGet]
        public string Get()
        {
            return "Hello World, Welcome API!";
        }
    }
}
