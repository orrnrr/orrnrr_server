using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using OrrnrrWebApi.Exceptions;

namespace OrrnrrWebApi.Parameters
{
    public class SortingParameter
    {
        [FromQuery(Name = "orderBy")]
        public string? OrderBy { get; set; }
        [FromQuery(Name = "sorting")]
        public string? Sorting { get; set; }

        internal IComparer<T>? GetComparer<T>() where T : IComparable
        {
            string sorting = GetSortingOrDefault();

            switch (sorting)
            {
                case Constants.Sorting.ASC:
                    return Comparer<T>.Default;
                case Constants.Sorting.DESC:
                    return Comparer<T>.Create((x, y) => y.CompareTo(x));
                default:
                    return null;
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
