using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
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
        public IActionResult GetTokenList([FromQuery] PagenationParameter pagination, [FromQuery]string? orderBy, [FromQuery]string? sorting)
        {
            pagination.ThrowIfNotValid();

            if (string.IsNullOrEmpty(orderBy))
            {
                TokenService.GetTokensOrderByTradeAmount();
            }

            IEnumerable<TokenResponse> response;
            
            switch (orderBy)
            {
                case "TARDE_AMONT":
                    response = TokenService.GetTokensOrderByTradeAmount();
                    break;
                case "CURRENT_PRICE":
                    response = TokenService.GetTokensOrderByCurrentPrice();
                    break;
                case "CHANGE_RATE":
                    response = TokenService.GetTokensOrderByChangeRate();
                    break;
                default:
                    throw new BadRequestApiException("orderBy의 값은 'TARDE_AMONT', 'CURRENT_PRICE', 'CHANGE_RATE' 외에 다른 값이 될 수 없습니다.");
            }

            return Ok(response);
        }
    }
}
