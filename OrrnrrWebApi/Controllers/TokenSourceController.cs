using DataAccessLib.Models;
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
    public class TokenSourceController : Controller
    {
        private OrrnrrContext OrrnrrContext { get => ContextManager.Instance.OrrnrrContext; }

        [HttpPost]
        [Consumes("application/json")]
        public IActionResult CreateTokenSource([FromBody] TokenSourceRequest request)
        {
            using (var context = OrrnrrContext)
            {
                var existsTokenSource = context.TokenSources
                    .Where(x => x.Name.Equals(request.Name))
                    .FirstOrDefault();

                if (existsTokenSource is not null)
                {
                    Console.WriteLine($"existsTokenSource.Id = {existsTokenSource.Id}");
                    throw new ApiException(HttpStatusCode.Conflict, "같은 이름의 토큰소스가 이미 존재합니다.");
                }

                var newTokenSource = request.ToTokenSource();

                context.TokenSources.Add(newTokenSource);
                context.SaveChanges();

                Console.WriteLine($"newTokenSource.Id = {newTokenSource.Id}");
                //return new CreatedResult("tokensource", TokenSourceResponse.From(newTokenSource));
                return Created("/tokensource", TokenSourceResponse.From(newTokenSource));
            }
        }
    }
}
