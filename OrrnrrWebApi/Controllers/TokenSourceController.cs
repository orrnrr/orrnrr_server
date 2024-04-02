using DataAccessLib.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using OrrnrrWebApi.Converts;
using OrrnrrWebApi.Exceptions;
using OrrnrrWebApi.Requests;
using System.Net;

namespace OrrnrrWebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TokenSourceController
    {
        private OrrnrrContext OrrnrrContext { get => OrrnrrContext.Instance; }

        [HttpPost]
        public IActionResult CreateTokenSource([FromBody]CreateTokenSourceRequest request)
        {
            var newTokenSource = request.ToTokenSource();

            OrrnrrContext.TokenSources.Add(newTokenSource);
            OrrnrrContext.SaveChanges();

#error 반환 무엇으로 할 것인지 코드 작성 필요
        }
    }
}
