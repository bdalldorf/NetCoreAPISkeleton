using System.Collections.Generic;
using bdNetCoreAPIDataTransfer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace bdNetCoreAPI.Controllers
{
    /// <summary>
    /// The HttpGet, HttpPost, HttpPut, HttpDelete Attributes aren't necessary for the methods the way I am using the routes.
    /// But if you added a base route to the class Route Attribute they can be used properly.
    /// </summary>
    [Authorize]
    [Route("")]
    [ApiController]
    public class AttributeRouteController : Controller
    {
        [Route(ApiRoutes.ExampleRoute.GetExampleItems)]
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "List Item 1", "List Item 2" };
        }

        [Route(ApiRoutes.ExampleRoute.GetExampleItem)]
        [HttpGet]
        public string Get(int id)
        {
            return "Item";
        }

        [Route(ApiRoutes.ExampleRoute.SaveExampleItem)]
        [HttpPost]
        public void Post([FromBody]string value)
        {

        }

        [Route(ApiRoutes.ExampleRoute.UpdateExampleItem)]
        [HttpPut]
        public void Put(int id, [FromBody]string value)
        {
        }

        [Route(ApiRoutes.ExampleRoute.DeleteExampleItem)]
        [HttpDelete]
        public void Delete(int id)
        {
        }
    }
}
