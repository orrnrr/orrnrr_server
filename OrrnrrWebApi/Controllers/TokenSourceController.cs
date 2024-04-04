using DataAccessLib.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using OrrnrrWebApi.Converts;
using OrrnrrWebApi.Exceptions;
using OrrnrrWebApi.Requests;
using OrrnrrWebApi.Responses;
using System.Net;

namespace OrrnrrWebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TokenSourceController
    {
        private OrrnrrContext OrrnrrContext { get => OrrnrrContext.Instance; }

        //[HttpPost]
        //[Consumes("application/json")]
        //public IActionResult CreateTokenSource([FromBody]TokenSourceRequest request)
        //{
        //    var existsTokenSource = OrrnrrContext.TokenSources
        //        .Where(x => x.Name.Equals(request.Name))
        //        .FirstOrDefault();

        //    if (existsTokenSource is not null)
        //    {
        //        throw new ApiException(HttpStatusCode.Conflict, "같은 이름의 토큰소스가 이미 존재합니다.");
        //    }

        //    var newTokenSource = request.ToTokenSource();

        //    OrrnrrContext.TokenSources.Add(newTokenSource);
        //    OrrnrrContext.SaveChanges();

        //    return new CreatedResult("tokensource", TokenSourceResponse.From(newTokenSource));
        //}
    }
}
