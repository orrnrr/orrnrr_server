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
        public IActionResult GetTokenList([FromQuery]int page, [FromQuery]int size, [FromQuery]string? orderBy, [FromQuery]string? orderDirection)
        {
            if (page < 1)
            {
                throw new BadRequestApiException("page의 값은 1이상이어야 합니다.");
            }
            if (size < 1)
            {
                throw new BadRequestApiException("size의 값은 1이상이어야 합니다.");
            }

            if (string.IsNullOrEmpty(orderBy))
            {
                orderBy = OrderByConsts.TARDE_AMOUNT;
            }
            if (string.IsNullOrEmpty(orderDirection))
            {
                orderDirection = OrderDirectionConsts.ASC;
            }

            Paging pagination = Paging.Of(page, size);
            Ordering ordering = Ordering.Of(orderBy, orderDirection);

            sortingParam.ThrowIfNotValid();

            string sorting = sortingParam.GetSortingOrDefault();
            if (!OrderDirectionConsts.Contains(sorting))
            {
                throw new BadRequestApiException($"sorting의 값은 '{OrderDirectionConsts.ASC}', '{OrderDirectionConsts.DESC}' 외에 다른 값이 될 수 없습니다.");
            }

            IEnumerable<TokenResponse> response;
            switch (sortingParam.GetOrderByOrDefault(OrderByConsts.TARDE_AMOUNT))
            {
                case OrderByConsts.TARDE_AMOUNT:
                    {
                        var comparer = sortingParam.GetComparer<int>();
                        if (comparer is null)
                        {

                        }
                        response = TokenService.GetTokensOrderByTradeAmount(comparer);
                        break;
                    }
                case OrderByConsts.CURRENT_PRICE:
                    {
                        var comparer = sortingParam.GetComparer<int>();
                        response = TokenService.GetTokensOrderByCurrentPrice(comparer);
                        break;
                    }
                case OrderByConsts.CHANGE_RATE:
                    {
                        var comparer = sortingParam.GetComparer<double>();
                        response = TokenService.GetTokensOrderByChangeRate(comparer);
                        break;
                    }
                default:
                    throw new BadRequestApiException($"orderBy의 값은 '{OrderByConsts.TARDE_AMOUNT}', '{OrderByConsts.CURRENT_PRICE}', '{OrderByConsts.CHANGE_RATE}' 외에 다른 값이 될 수 없습니다.");
            }

            return Ok(response);
        }
    }
}
