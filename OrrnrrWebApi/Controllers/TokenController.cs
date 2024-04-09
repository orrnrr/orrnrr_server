using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using OrrnrrWebApi.Exceptions;
using OrrnrrWebApi.Pagination;
using OrrnrrWebApi.Responses;
using OrrnrrWebApi.Services;
using OrrnrrWebApi.Sort;
using System.ComponentModel.DataAnnotations;
using System.Net.NetworkInformation;
using System.Text.Json;

namespace OrrnrrWebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TokenController : Controller
    {
        private ITokenService TokenService { get; }

        public TokenController(ITokenService tokenService)
        {
            TokenService = tokenService;
        }

        [HttpGet]
        public IActionResult GetTokenList([FromQuery] int page, [FromQuery] int size, [FromQuery] string? orderBy, [FromQuery] string? orderDirection)
        {
            if (page < 1)
            {
                throw new BadRequestApiException($"{nameof(page)}의 값은 1이상이어야 합니다.");
            }
            if (size < 1)
            {
                throw new BadRequestApiException($"{nameof(size)}의 값은 1이상이어야 합니다.");
            }

            var orderByType = OrderByConsts.GetOrderByTypeOrDefault(orderBy, OrderByType.거래대금);
            if (orderByType == OrderByType.None)
            {
                throw new BadRequestApiException($"{nameof(orderBy)}의 값이 유효하지 않습니다.");
            }

            var orderDirectionType = OrderDirectionConsts.GetOrderDirectionTypeOrDefault(orderDirection, OrderDirectionType.Ascending);
            if (orderDirectionType == OrderDirectionType.None)
            {
                throw new BadRequestApiException($"{nameof(orderDirection)}의 값이 유효하지 않습니다.");
            }

            var paging = Paging.Of(page, size);
            var ordering = Ordering.Of(orderByType, orderDirectionType);

            var response = ordering.OrderBy switch
            {
                OrderByType.거래대금 => TokenService.GetTokensOrderByTradeAmount(paging, ordering),
                OrderByType.현재가 => TokenService.GetTokensOrderByCurrentPrice(paging, ordering),
                OrderByType.전일대비변동률 => TokenService.GetTokensOrderByChangeRate(paging, ordering),
                _ => throw new BadRequestApiException($"{nameof(orderBy)}의 값이 유효하지 않습니다."),
            };
            return Ok(response);
        }
    }
}
