using System.Collections.Generic;
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
    public class APITestController : Controller
    {
        [Route(ApiRoutes.ApiTestRoute.GetApiTestItems)]
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "List Item 1", "List Item 2" };
        }

        [Route(ApiRoutes.ApiTestRoute.GetApiTestItem)]
        [HttpGet]
        public string Get(int id)
        {
            return $"Item {id}";
        }

        [Route(ApiRoutes.ApiTestRoute.SaveApiTestItem)]
        [HttpPost]
        public void Post([FromBody]string value)
        {

        }

        [Route(ApiRoutes.ApiTestRoute.UpdateApiTestItem)]
        [HttpPut]
        public void Put(int id, [FromBody]string value)
        {
        }

        [Route(ApiRoutes.ApiTestRoute.DeleteApiTestItem)]
        [HttpDelete]
        public void Delete(int id)
        {
        }
    }
}