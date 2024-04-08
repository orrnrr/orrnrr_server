using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using OrrnrrWebApi.Constants;
using OrrnrrWebApi.Exceptions;
using OrrnrrWebApi.Parameters;
using OrrnrrWebApi.Responses;
using OrrnrrWebApi.Services;
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
        public IActionResult GetTokenList([FromQuery] PagenationParameter pagination, [FromQuery] SortingParameter sortingParam)
        {
            pagination.ThrowIfNotValid();
            sortingParam.ThrowIfNotValid();

            string sorting = sortingParam.GetSortingOrDefault();
            if (!Sorting.Contains(sorting))
            {
                throw new BadRequestApiException($"sorting의 값은 '{Sorting.ASC}', '{Sorting.DESC}' 외에 다른 값이 될 수 없습니다.");
            }

            IEnumerable<TokenResponse> response;
            switch (sortingParam.GetOrderByOrDefault(OrderBy.TARDE_AMONT))
            {
                case OrderBy.TARDE_AMONT:
                    {
                        var comparer = sortingParam.GetComparer<int>();
                        if (comparer is null)
                        {

                        }
                        response = TokenService.GetTokensOrderByTradeAmount(comparer);
                        break;
                    }
                case OrderBy.CURRENT_PRICE:
                    {
                        var comparer = sortingParam.GetComparer<int>();
                        response = TokenService.GetTokensOrderByCurrentPrice(comparer);
                        break;
                    }
                case OrderBy.CHANGE_RATE:
                    {
                        var comparer = sortingParam.GetComparer<double>();
                        response = TokenService.GetTokensOrderByChangeRate(comparer);
                        break;
                    }
                default:
                    throw new BadRequestApiException($"orderBy의 값은 '{OrderBy.TARDE_AMONT}', '{OrderBy.CURRENT_PRICE}', '{OrderBy.CHANGE_RATE}' 외에 다른 값이 될 수 없습니다.");
            }

            return Ok(response);
        }
    }
}
