using DataAccessLib.Types;
using Npgsql.Internal;
using System;
using System.Collections.Generic;

namespace DataAccessLib.Models;

public partial class TransactionHistory
{
    public TransactionHistory() { }

    public TransactionHistory(TokenOrderHistory buyOrder, TokenOrderHistory sellOrder, TradeAction tradeAction, int transactionCount)
    {
        BuyOrderId = buyOrder.Id;
        SellOrderId = sellOrder.Id;
        TradeActionId = tradeAction.Id;
        BuyOrder = buyOrder;
        SellOrder = sellOrder;
        TradeAction = tradeAction;
        TransactionCount = transactionCount;
    }

    public int Id { get; set; }

    public int BuyOrderId { get; set; }

    public int SellOrderId { get; set; }

    public byte TradeActionId { get; set; }

    public int TransactionCount { get; set; }

    public DateTime TransactionDateTime { get; set; } = DateTime.UtcNow;

    public virtual TradeAction TradeAction { get; set; } = null!;

    public virtual TokenOrderHistory BuyOrder { get; set; } = null!;

    public virtual TokenOrderHistory SellOrder { get; set; } = null!;
}
