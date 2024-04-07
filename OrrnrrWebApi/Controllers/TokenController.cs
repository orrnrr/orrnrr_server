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
        public IActionResult GetTokenList([FromQuery] PagenationParameter pagination, [FromQuery]SortingParameter sortingParam)
        {
            pagination.ThrowIfNotValid();

            string sorting = sortingParam.GetSortingOrDefault();
            if(!Sorting.Contains(sorting))
            {
                throw new BadRequestApiException($"sorting의 값은 '{Sorting.ASC}', '{Sorting.DESC}' 외에 다른 값이 될 수 없습니다.");
            }

            var comparer = sortingParam.GetComparerOrDefault<int>();

            IEnumerable<TokenResponse> response;
            switch (sortingParam.GetOrderByOrDefault(OrderBy.TARDE_AMONT))
            {
                case OrderBy.TARDE_AMONT:
                    response = TokenService.GetTokensOrderByTradeAmount();
                    break;
                case OrderBy.CURRENT_PRICE:
                    response = TokenService.GetTokensOrderByCurrentPrice();
                    break;
                case OrderBy.CHANGE_RATE:
                    response = TokenService.GetTokensOrderByChangeRate();
                    break;
                default:
                    throw new BadRequestApiException($"orderBy의 값은 '{OrderBy.TARDE_AMONT}', '{OrderBy.CURRENT_PRICE}', '{OrderBy.CHANGE_RATE}' 외에 다른 값이 될 수 없습니다.");
            }

            return Ok(response);
        }
    }
}
