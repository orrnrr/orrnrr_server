


namespace OrrnrrWebApi.Sort
{
    public static class OrderByConsts
    {
        public const string TARDE_AMOUNT = "TARDE_AMOUNT";
        public const string CURRENT_PRICE = "CURRENT_PRICE";
        public const string CHANGE_RATE = "CHANGE_RATE";

        internal static bool Contains(string orderBy)
        {
            switch (orderBy)
            {
                case TARDE_AMOUNT:
                case CURRENT_PRICE:
                case CHANGE_RATE:
                    return true;
                default:
                    return false;
            }
        }

        internal static OrderByType GetOrderByType(string orderByStr)
        {
            switch (orderByStr)
            {
                case CURRENT_PRICE:
                    return OrderByType.현재가;
                case TARDE_AMOUNT:
                    return OrderByType.거래대금;
                case CHANGE_RATE:
                    return OrderByType.전일대비변동률;
                default:
                    return OrderByType.None;
            }
        }

        /// <summary>
        /// 주어진 문자열에 대응되는 OrderByType을 반환합니다. 만약 문자열이 null이거나 빈 문자열이면 기본값으로 주어진 값을 반환하고, 대응되는 값이 없는 경우 None을 반환합니다.
        /// </summary>
        /// <param name="orderByStr"></param>
        /// <param name="defaultOrderByType"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal static OrderByType GetOrderByTypeOrDefault(string? orderByStr, OrderByType defaultOrderByType)
        {
            if (string.IsNullOrEmpty(orderByStr))
            {
                return defaultOrderByType;
            }

            return GetOrderByType(orderByStr);
        }
    }
}
