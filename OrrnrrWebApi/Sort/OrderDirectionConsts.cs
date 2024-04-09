

namespace OrrnrrWebApi.Sort
{
    public static class OrderDirectionConsts
    {
        public const string ASC = "ASC";
        public const string DESC = "DESC";

        internal static bool Contains(string sorting)
        {
            switch (sorting)
            {
                case ASC:
                case DESC:
                    return true;
                default:
                    return false;
            }
        }

        internal static OrderDirectionType GetOrderDirectionType(string orderDirectionStr)
        {
            switch (orderDirectionStr)
            {
                case ASC:
                    return OrderDirectionType.Ascending;
                case DESC:
                    return OrderDirectionType.Descending;
                default:
                    return OrderDirectionType.None;
            }
        }

        /// <summary>
        /// 주어진 문자열에 해당하는 OrderDirectionType을 반환합니다. 만약 주어진 문자열이 null이거나 빈 문자열이면 기본값으로 제공한 값을 반환합니다. 만약 주어진 문자열에 대응되는 값이 없다면 None을 반환합니다.
        /// </summary>
        /// <param name="orderDirectionStr"></param>
        /// <param name="ascending"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal static OrderDirectionType GetOrderDirectionTypeOrDefault(string? orderDirectionStr, OrderDirectionType defaultOrderDirectionType = OrderDirectionType.Ascending)
        {
            if (string.IsNullOrEmpty(orderDirectionStr))
            {
                return defaultOrderDirectionType;
            }

            return GetOrderDirectionType(orderDirectionStr);
        }
    }
}
