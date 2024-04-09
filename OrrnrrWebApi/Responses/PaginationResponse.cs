using OrrnrrWebApi.Pagination;

namespace OrrnrrWebApi.Responses
{
    public class PaginationResponse<T>
    {
        private PaginationResponse(Paging paging, IEnumerable<T> data, int total)
        {
            Page = paging.Page;
            Size = paging.Size;
            TotalElement = total;
            Data = data;
        }

        public int Page { get; }
        public int Size { get; }
        public int TotalElement { get; }
        public IEnumerable<T> Data { get; }

        public static PaginationResponse<T> Of(Paging paging, IEnumerable<T> data, int total)
        {
            return new PaginationResponse<T>(paging, data, total);
        }
    }
}
