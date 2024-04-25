namespace DataAccessLib.Models
{
    public partial class OrderType
    {
        public OrderType() { }

        public byte Id { get; set; }
        public string Name { get; set; } = null!;
    }

    public enum OrderCategory : byte
    {
        None = 0,
        Limit = 1,
        Market = 2,
        Reservation = 3,
    }

    public static class OrderCategoryExtensions
    {
        public static string GetOrderTypeName(this OrderCategory orderType)
        {
            return orderType switch
            {
                OrderCategory.Limit => "지정가",
                OrderCategory.Market => "시장가",
                OrderCategory.Reservation => "예약",
                _ => "",
            };
        }
    }
}
