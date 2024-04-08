

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

        internal static OrderByType ParseOrderByType(string orderBy)
        {
            switch (orderBy)
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
    }
}
