using Microsoft.AspNetCore.Mvc;
using OrrnrrWebApi.Exceptions;
using System.Text.Json.Serialization;

namespace OrrnrrWebApi.Parameters
{
    public class PagenationParameter
    {
        [FromQuery(Name = "page")]
        public int Page { get; set; }
        [FromQuery(Name = "size")]
        public int Size { get; set; }

        internal void ThrowIfNotValid()
        {
            if (Page < 1)
            {
                throw new BadRequestApiException("page의 값은 1이상이어야 합니다.");
            }

            if (Size < 1)
            {
                throw new BadRequestApiException("size의 값은 1이상이어야 합니다.");
            }
        }
    }
}
