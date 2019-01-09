using System.Collections.Generic;
using bdNetCoreAPIDataTransfer;
using bdNetCoreAPIDataTransfer.Models;
using Microsoft.AspNetCore.Authorization;
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
        public List<ItemModel> GetAll()
        {
            return new List<ItemModel>
            {
                new ItemModel() { ID = 1, ItemName = "Dashboard Item 1", ItemPrice = 1.50m  },
                new ItemModel() { ID = 2, ItemName = "Dashboard Item 2", ItemPrice = 4.50m  },
                new ItemModel() { ID = 3, ItemName = "Dashboard Item 3", ItemPrice = 2.75m  },
            };
        }

        [Route(ApiRoutes.ApiTestRoute.GetApiTestItem)]
        [HttpGet]
        public string Get(int id)
        {
            return $"Item {id}";
        }

        [AllowAnonymous]
        [Route(ApiRoutes.ApiTestRoute.SaveApiTestItem)]
        [HttpPost]
        public IActionResult Post([FromBody]int id)
        {
            if (id != 0)
            {

                return Ok(id);
            }

            return BadRequest(id);
        }

        [Route(ApiRoutes.ApiTestRoute.UpdateApiTestItem)]
        [HttpPut]
        public void Put(int id, string value)
        {
        }

        [Route(ApiRoutes.ApiTestRoute.DeleteApiTestItem)]
        [HttpDelete]
        public void Delete(int id)
        {
        }
    }
}