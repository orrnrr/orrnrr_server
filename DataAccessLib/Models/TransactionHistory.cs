using DataAccessLib.Types;
using Npgsql.Internal;
using System;
using System.Collections.Generic;

namespace DataAccessLib.Models;

public partial class TransactionHistory
{
    public TransactionHistory(TokenOrderHistory buyOrder, TokenOrderHistory sellOrder, TradeActionType tradeActionType, int transactionCount)
        : this(buyOrder.Id, sellOrder.Id, buyOrder, sellOrder, tradeActionType, transactionCount)
    {
    }

    public TransactionHistory(int buyOrderId, int sellOrderId, TokenOrderHistory buyOrder, TokenOrderHistory sellOrder, TradeActionType tradeActionType, int transactionCount)
    {
        BuyOrderId = buyOrderId;
        SellOrderId = sellOrderId;
        BuyOrder = buyOrder;
        SellOrder = sellOrder;
        TradeActionType = tradeActionType;
        TransactionCount = transactionCount;
    }

    public int Id { get; set; }

    public int BuyOrderId { get; set; }

    public int SellOrderId { get; set; }

    public int TransactionCount { get; set; }

    public byte TradeAction { get; set; }

    public DateTime TransactionDateTime { get; set; }

    public virtual TokenOrderHistory BuyOrder { get; set; } = null!;

    public virtual TokenOrderHistory SellOrder { get; set; } = null!;

    public TradeActionType TradeActionType
    {
        get => (TradeActionType)TradeAction;
        set => TradeAction = (byte)value;
    }

    public static TransactionHistory Sign(TokenOrderHistory order1, TokenOrderHistory order2)
    {
        if (order1.IsBuyOrder == order2.IsBuyOrder)
        {
            throw new InvalidOperationException("주문을 체결할 수 없습니다. 두 주문이 모두 매수 주문이거나 매도 주문입니다.");
        }

        int transactionCount = order1.Trade(order2);

        TokenOrderHistory buyOrder;
        TokenOrderHistory sellOrder;

        if (order1.IsBuyOrder)
        {
            buyOrder = order1;
            sellOrder = order2;
        }
        else
        {
            buyOrder = order2;
            sellOrder = order1;
        }

        TradeActionType tradeActionType = buyOrder.ExecutableCount > 0 ? TradeActionType.Buy : 
            sellOrder.ExecutableCount > 0 ? TradeActionType.Sell :
            TradeActionType.Hold;

        return new TransactionHistory(buyOrder, sellOrder, tradeActionType, transactionCount);
    }
}
