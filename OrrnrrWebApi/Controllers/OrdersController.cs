using DataAccessLib.Models;
using Microsoft.AspNetCore.Mvc;
using OrrnrrWebApi.Requests;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OrrnrrWebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class OrdersController : Controller
    {
        //private IOrdersService OrdersService { get; }

        //public OrdersController(IOrdersService ordersService)
        //{
        //    OrdersService = ordersService ?? throw new ArgumentNullException(nameof(ordersService));
        //}

        [HttpPost]
        public IActionResult CreateOrder([FromBody] CreateOrderRequest request)
        {
            request.ThrowIfNotValid();
            //var request = new CreateOrderRequest(args);

            //var newOrder = new TokenOrderHistory();


            //var OrdersService.CreateOrder(request);

            return Ok(JsonSerializer.Serialize(request));
        }
    }
}
