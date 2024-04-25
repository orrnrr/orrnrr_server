using DataAccessLib.Models;
using Microsoft.AspNetCore.Mvc;
using OrrnrrWebApi.Authorization;
using OrrnrrWebApi.Exceptions;
using OrrnrrWebApi.Requests;
using OrrnrrWebApi.Responses;
using OrrnrrWebApi.Services;
using OrrnrrWebApi.Types;
using OrrnrrWebApi.Utils;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OrrnrrWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrdersController : Controller
    {
        private IOrdersService OrdersService { get; }
        private ITokenService TokenService { get; }
        private IUserService UserService { get; }

        public OrdersController(IOrdersService ordersService, ITokenService tokenService, IUserService userService)
        {
            OrdersService = ordersService;
            TokenService = tokenService;
            UserService = userService;
        }

        [HttpPost]
        [RequireAccessToken(UserRoles.User)]
        [Consumes("application/x-www-form-urlencoded")]
        public IActionResult CreateOrder([FromForm] int? tokenId, [FromForm] string? orderType, [FromForm] bool? isBuyOrder, [FromForm] int? price, [FromForm] int? count)
        {
            var isValidOrderType = Enum.TryParse(orderType ?? throw new BadRequestApiException($"{nameof(orderType)}은 null일 수 없습니다."), true, out OrderType validOrderType);
            if (!isValidOrderType)
            {
                throw OrderTypeExtensions.CreateValidRangeApiException(nameof(orderType));
            }

            if (validOrderType != OrderType.Market)
            {
                if ((price ?? throw new NullParameterApiException(nameof(price))) <= 0)
                {
                    throw new NonPositiveNumberApiException(nameof(price));
                }
            }

            if ((count ?? throw new NonPositiveNumberApiException(nameof(count))) <= 0)
            {
                throw new NonPositiveNumberApiException(nameof(count));
            }

            TokenOrderHistory createdTokenOrderHistory = validOrderType switch
            {
                OrderType.Market => OrdersService.CreateMarketOrder(
                        userId: AuthUtil.GetUserId(HttpContext.User)
                        , tokenId: tokenId ?? throw new NullParameterApiException(nameof(tokenId))
                        , isBuyOrder: isBuyOrder ?? throw new NullParameterApiException(nameof(isBuyOrder))
                        , count: count.Value
                    ),
                OrderType.Limit => OrdersService.CreateLimitOrder(
                        userId: AuthUtil.GetUserId(HttpContext.User)
                        , tokenId: tokenId ?? throw new NullParameterApiException(nameof(tokenId))
                        , isBuyOrder: isBuyOrder ?? throw new NullParameterApiException(nameof(isBuyOrder))
                        , price: price!.Value
                        , count: count.Value
                    ),
                _ => throw OrderTypeExtensions.CreateValidRangeApiException(nameof(orderType))
            };

            return Created("/orders", TokenOrderHistoryResponse.From(createdTokenOrderHistory));
        }
    }
}
