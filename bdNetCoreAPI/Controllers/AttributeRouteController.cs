using System.Collections.Generic;
using bdNetCoreAPIDataTransfer;
using bdNetCoreAPIDataTransfer.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace bdNetCoreAPI.Controllers
{
    /// <summary>
    /// The HttpGet, HttpPost, HttpPut, HttpDelete Attributes aren't necessary for the methods the way I am using the routes.
    /// But if you added a base route to the class Route Attribute they can be used properly.
    /// </summary>
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    [ValidateAntiForgeryToken]
    [Route("")]
    [ApiController]
    public class AttributeRouteController : Controller
    {
        [Route(ApiRoutes.ExampleRoute.GetExampleItems)]
        [HttpGet]
        public IEnumerable<ItemModel> Get()
        {
            return new List<ItemModel>
            {
                new ItemModel() { ID = 1, ItemName = "Item 1", ItemPrice = 1.50m  },
                new ItemModel() { ID = 2, ItemName = "Item 2", ItemPrice = 4.50m  },
                new ItemModel() { ID = 3, ItemName = "Item 3", ItemPrice = 2.75m  },
            };
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
