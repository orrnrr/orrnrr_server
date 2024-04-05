using System;
using System.Collections.Generic;

namespace DataAccessLib.Models;

public partial class TransactionHistory
{
    public int Id { get; set; }

    public int BuyOrderId { get; set; }

    public int SellOrderId { get; set; }

    public int TransactionCount { get; set; }

    public bool IsBuyTransaction { get; set; }

    public DateTime TransactionDateTime { get; set; }

    public virtual TokenOrderHistory BuyOrder { get; set; } = null!;

    public virtual TokenOrderHistory SellOrder { get; set; } = null!;
}
