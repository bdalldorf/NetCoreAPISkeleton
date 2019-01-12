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
    [Authorize]
    [AutoValidateAntiforgeryToken]
    [ApiController]
    [Route("")]
    public class APITestController : Controller
    {
        [HttpGet]
        [Route(ApiRoutes.ApiTestRoute.GetApiTestItems)]
        public List<ItemModel> GetAll()
        {
            return new List<ItemModel>
            {
                new ItemModel() { ID = 1, ItemName = "Dashboard Item 1", ItemPrice = 1.50m  },
                new ItemModel() { ID = 2, ItemName = "Dashboard Item 2", ItemPrice = 4.50m  },
                new ItemModel() { ID = 3, ItemName = "Dashboard Item 3", ItemPrice = 2.75m  },
            };
        }

        [HttpGet]
        [Route(ApiRoutes.ApiTestRoute.GetApiTestItem)]
        public string Get(int id)
        {
            return $"Item {id}";
        }

        [HttpPost]
        [Route(ApiRoutes.ApiTestRoute.SaveApiTestItem)]
        public IActionResult Post([FromBody]int id)
        {
            if (id != 0)
            {

                return Ok(id);
            }

            return BadRequest(id);
        }

        [HttpPut]
        [Route(ApiRoutes.ApiTestRoute.UpdateApiTestItem)]
        public void Put(int id, string value)
        {
        }

        [HttpDelete]
        [Route(ApiRoutes.ApiTestRoute.DeleteApiTestItem)]
        public void Delete(int id)
        {
        }
    }
}