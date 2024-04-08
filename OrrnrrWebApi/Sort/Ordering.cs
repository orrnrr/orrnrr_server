
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

        internal static Ordering Of(string orderBy, string orderDirection)
        {
            var orderByType = OrderByConsts.ParseOrderByType(orderBy);
            if (orderByType == OrderByType.None)
            {
                throw new ArgumentOutOfRangeException(nameof(orderBy), orderBy, $"{nameof(orderBy)}의 값이 유효하지 않습니다.");
            }

            var orderDirectionType = OrderDirectionConsts.ParseOrderByType(orderDirection);
            if (orderDirectionType == OrderDirectionType.None)
            {
                throw new ArgumentOutOfRangeException(nameof(orderDirection), orderDirection, $"{nameof(orderDirection)}의 값이 유효하지 않습니다.");
            }

            return new Ordering(orderByType, orderDirectionType);
        }
    }
}
