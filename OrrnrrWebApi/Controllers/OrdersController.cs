using DataAccessLib.Models;
using Microsoft.AspNetCore.Mvc;
using OrrnrrWebApi.Authorization;
using OrrnrrWebApi.Exceptions;
using OrrnrrWebApi.Requests;
using OrrnrrWebApi.Services;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OrrnrrWebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class OrdersController : Controller
    {
        private IOrdersService OrdersService { get; }

        public OrdersController(IOrdersService ordersService)
        {
            OrdersService = ordersService ?? throw new ArgumentNullException(nameof(ordersService));
        }

        [HttpPost]
        [RequireAccessToken(UserRoles.User)]
        public IActionResult CreateOrder([FromBody] CreateOrderRequest request)
        {
            request.ThrowIfNotValid();

            TokenOrderHistory tokenOrderHistory = OrdersService.CreateOrder(
                tokenId: request.TokenId ?? throw new BadRequestApiException("tokenId는 null일 수 없습니다.")
                
            );



            return Ok(JsonSerializer.Serialize(request));
        }
    }
}
