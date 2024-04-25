using DataAccessLib.Types;
using Npgsql.Internal;
using System;
using System.Collections.Generic;

namespace DataAccessLib.Models;

public partial class TransactionHistory
{
    public TransactionHistory() { }

    public TransactionHistory(TokenOrderHistory order1, TokenOrderHistory order2, TradeAction tradeAction, int transactionCount, int signedPrice)
    {
        var buyOrder = order1.IsBuyOrder ? order1 : order2;
        var sellOrder = order1.IsBuyOrder ? order2 : order1;

        BuyOrderId = buyOrder.Id;
        SellOrderId = sellOrder.Id;
        BuyOrder = buyOrder;
        SellOrder = sellOrder;
        TradeActionId = tradeAction.Id;
        TradeAction = tradeAction;
        TransactionCount = transactionCount;
        SignedPrice = signedPrice;
    }

    public int Id { get; set; }

    public int BuyOrderId { get; set; }

    public int SellOrderId { get; set; }

    public byte TradeActionId { get; set; }

    public int TransactionCount { get; set; }

    public int SignedPrice { get; set; }

    public DateTime TransactionDateTime { get; set; } = DateTime.UtcNow;

    public virtual TradeAction TradeAction { get; set; } = null!;

    public virtual TokenOrderHistory BuyOrder { get; set; } = null!;

    public virtual TokenOrderHistory SellOrder { get; set; } = null!;
}
