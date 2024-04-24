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
        if (order1.IsBuyOrder == order2.IsBuyOrder)
        {
            throw new InvalidOperationException("두 주문 모두 매수주문이거나 매도주문이므로 체결할 수 없습니다.");
        }

        BuyOrderId = order1.IsBuyOrder? order1.Id : order2.Id;
        SellOrderId = order1.IsBuyOrder? order2.Id : order1.Id;
        BuyOrder = order1.IsBuyOrder? order1 : order2;
        SellOrder = order1.IsBuyOrder ? order2 : order1;
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
