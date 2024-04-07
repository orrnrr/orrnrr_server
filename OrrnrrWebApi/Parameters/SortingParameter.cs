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

        internal IComparer<T> GetComparerOrDefault<T>()
        {
            string sorting = GetSortingOrDefault();

            switch () { 

            }
        }

        internal string GetOrderByOrDefault(string defaultOrderBy)
        {
            return string.IsNullOrEmpty(OrderBy) ? defaultOrderBy : OrderBy;
        }

        internal string GetSortingOrDefault(string defulatSorting = Constants.Sorting.ASC)
        {
            return string.IsNullOrEmpty(Sorting) ? defulatSorting : Sorting!;
        }
    }
}
