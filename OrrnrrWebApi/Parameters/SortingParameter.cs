using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace OrrnrrWebApi.Parameters
{
    public class SortingParameter
    {
        [FromQuery(Name = "orderBy")]
        public string? OrderBy { get; set; }
        [FromQuery(Name = "sorting")]
        public string? Sorting { get; set; }
    }
}
