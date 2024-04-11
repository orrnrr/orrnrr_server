using DataAccessLib.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using OrrnrrWebApi.Authorization;
using OrrnrrWebApi.Exceptions;
using OrrnrrWebApi.Pagination;
using OrrnrrWebApi.Responses;
using OrrnrrWebApi.Services;
using OrrnrrWebApi.Sort;
using OrrnrrWebApi.Utils;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Text.Json;

namespace OrrnrrWebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TokensController : Controller
    {
        private ITokenService TokenService { get; }
        private ITokenSourceService TokenSourceService { get; }

        public TokensController(ITokenService tokenService, ITokenSourceService tokenSourceService)
        {
            TokenService = tokenService;
            TokenSourceService = tokenSourceService;
        }

        [HttpPost]
        //[RequireAccessToken(UserRoles.User | UserRoles.Manager)]
        public IActionResult CreateToken([FromForm]int tokenSourceId, [FromForm][MaxLength(200)] string? name, [FromForm][MaxLength(2000)] string? description)
        {
            if (!TokenSourceService.IsExistsById(tokenSourceId))
            {
                throw new BadRequestApiException("새로 생성될 토큰은 존재하지 않는 토큰소스를 참조할 수 없습니다.");
            }

            if (string.IsNullOrEmpty(name))
            {
                throw new BadRequestApiException("토큰의 이름은 null이거나 비어있을 수 없습니다.");
            }

            bool isExistsTokenName = TokenService.IsExistsTokenName(name);
            if (isExistsTokenName)
            {
                throw new ConflictApiException("이미 같은 이름의 토큰이 존재합니다.");
            }

            if (string.IsNullOrEmpty(description))
            {
                throw new BadRequestApiException("토큰에 대한 설명은 null이거나 비어있을 수 없습니다.");
            }

            var createdToken = TokenService.CreateToken(tokenSourceId, name, description);

            return Created("/tokens", SimpleTokenResponse.From(createdToken));
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
