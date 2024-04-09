
namespace OrrnrrWebApi.Sort
{
    public class Ordering
    {
        private Ordering(OrderByType orderBy, OrderDirectionType orderDirection)
        {
            OrderBy = orderBy;
            OrderDirection = orderDirection;
        }

        public OrderByType OrderBy { get; }
        public OrderDirectionType OrderDirection { get; }

        internal static Ordering Of(OrderByType orderByType, OrderDirectionType orderDirectionType)
        {
            if (orderByType == OrderByType.None)
            {
                throw new ArgumentOutOfRangeException(nameof(orderByType), orderByType, $"{nameof(orderByType)}의 값은 None이 될 수 없습니다.");
            }

            if (orderDirectionType == OrderDirectionType.None)
            {
                throw new ArgumentOutOfRangeException(nameof(orderDirectionType), orderDirectionType, $"{nameof(orderDirectionType)}의 값은 None이 될 수 없습니다.");
            }

            return new Ordering(orderByType, orderDirectionType);
        }
    }
}
