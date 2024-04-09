
namespace OrrnrrWebApi.Pagination
{
    public class Paging
    {
        private Paging(int page, int size)
        {
            Page = page;
            Size = size;
        }

        public int Page { get; }
        public int Size { get; }

        public int Skip => (Page - 1) * Size;

        internal static Paging Of(int page, int size)
        {
            if (page < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(page), page, $"{nameof(page)}의 값은 null이상이어야 합니다.");
            }

            if (size < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(size), size, $"{nameof(size)}의 값은 null이상이어야 합니다.");
            }

            return new Paging(page, size);
        }
    }
}
