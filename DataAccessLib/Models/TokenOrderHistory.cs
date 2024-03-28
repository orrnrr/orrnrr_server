using System;
using System.Collections.Generic;

namespace DataAccessLib.Models;

public partial record TokenOrderHistory
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int TokenId { get; set; }

    public bool IsBuyOrder { get; set; }

    public int OrderPrice { get; set; }

    public int OrderCount { get; set; }

    public int CompleteCount { get; set; }

    public DateOnly OrderDate { get; set; }

    public bool IsCanceled { get; set; }

    public virtual Token Token { get; set; } = null!;

    public virtual ICollection<TransactionHistory> TransactionHistoryBuyOrders { get; set; } = new List<TransactionHistory>();

    public virtual ICollection<TransactionHistory> TransactionHistorySellOrders { get; set; } = new List<TransactionHistory>();

    public virtual User User { get; set; } = null!;
}
