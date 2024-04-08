
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

        internal static OrderDirectionType ParseOrderByType(string orderDirection)
        {
            switch (orderDirection)
            {
                case ASC:
                    return OrderDirectionType.Ascending;
                case DESC:
                    return OrderDirectionType.Descending;
                default:
                    return OrderDirectionType.None;
            }
        }
    }
}
