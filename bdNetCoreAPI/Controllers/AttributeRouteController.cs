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
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    //[Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    [Authorize]
    [AutoValidateAntiforgeryToken]
    [ApiController]
    [Route("")]
    public class AttributeRouteController : Controller
    {
        [HttpGet]
        [Route(ApiRoutes.ExampleRoute.GetExampleItems)]
        public IEnumerable<ItemModel> Get()
        {
            return new List<ItemModel>
            {
                new ItemModel() { ID = 1, ItemName = "Lesson Item 1", ItemPrice = 1.50m  },
                new ItemModel() { ID = 2, ItemName = "Lesson Item 2", ItemPrice = 4.50m  },
                new ItemModel() { ID = 3, ItemName = "Lesson Item 3", ItemPrice = 2.75m  },
            };
        }

        [HttpGet]
        [Route(ApiRoutes.ExampleRoute.GetExampleItem)]
        public string Get(int id)
        {
            return "Item";
        }

        [HttpPost]
        [Route(ApiRoutes.ExampleRoute.SaveExampleItem)]
        public void Post([FromBody]string value)
        {

        }

        [HttpPut]
        [Route(ApiRoutes.ExampleRoute.UpdateExampleItem)]
        public void Put(int id, [FromBody]string value)
        {
        }

        [HttpDelete]
        [Route(ApiRoutes.ExampleRoute.DeleteExampleItem)]
        public void Delete(int id)
        {
        }
    }
}
