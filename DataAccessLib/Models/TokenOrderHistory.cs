﻿using DataAccessLib.Types;
using System;
using System.Collections.Generic;

namespace DataAccessLib.Models;

public partial class TokenOrderHistory
{
    public TokenOrderHistory() { }
    
    public TokenOrderHistory(User user, Token token, bool isBuyOrder, int count, int? price = null)
    {
        UserId = user.Id;
        TokenId = token.Id;
        User = user;
        Token = token;
        IsBuyOrder = isBuyOrder;
        OrderPrice = price;
        OrderCount = count;
    }

    public int Id { get; set; }

    public int UserId { get; set; }

    public int TokenId { get; set; }

    public bool IsBuyOrder { get; set; }

    public int? OrderPrice { get; set; } = null!;

    public int OrderCount { get; set; }

    public int CompleteCount { get; set; }

    public DateTime OrderDateTime { get; set; } = DateTime.UtcNow;

    public bool IsCanceled { get; set; }

    public virtual Token Token { get; set; } = null!;

    public virtual User User { get; set; } = null!;

    //public virtual ICollection<TransactionHistory> TransactionHistoryBuyOrders { get; set; } = new List<TransactionHistory>();

    //public virtual ICollection<TransactionHistory> TransactionHistorySellOrders { get; set; } = new List<TransactionHistory>();

    public int ExecutableCount { get => OrderCount - CompleteCount; }

    public static TokenOrderHistory CreateSellOrder(User user, Token token, int price, int count)
    {
        return new TokenOrderHistory(user, token, false, count, price);
    }

    public static TokenOrderHistory CreateBuyOrder(User user, Token token, int price, int count)
    {
        return new TokenOrderHistory(user, token, true, count, price);
    }

    public (int, TradeActionType) Sign(TokenOrderHistory other, int signedPrice)
    {
        if (IsCanceled || other.IsCanceled)
        {
            throw new InvalidOperationException("거래를 성사시킬 수 없습니다. 취소된 주문이 포함되어 있습니다.");
        }

        return (this.IsBuyOrder, other.IsBuyOrder) switch
        {
            (true, false) => this.Buy(other, signedPrice),
            (false, true) => other.Buy(this, signedPrice),
            _ => throw new InvalidOperationException("거래를 성사시킬 수 없습니다. 두 주문 모두 매수주문이거나 매도주문입니다.")
        };
    }

    private (int, TradeActionType) Buy(TokenOrderHistory other, int signedPrice)
    {
        int tradeCount = Math.Min(ExecutableCount, other.ExecutableCount);
        if (tradeCount <= 0)
        {
            throw new InvalidOperationException("거래를 성사시킬 수 없습니다. 두 주문의 거래가능 수량이 0 이하입니다.");
        }

        CompleteCount += tradeCount;
        other.CompleteCount += tradeCount;

        int payment = signedPrice * tradeCount;

        this.User.PayTo(other.User, payment);

        if (ExecutableCount > 0 && other.ExecutableCount > 0)
        {
            throw new InvalidOperationException("거래를 성공적으로 성사시키지 못했습니다.");
        }

        TradeActionType tradeActionType = this.ExecutableCount > 0 ? TradeActionType.매수 :
            other.ExecutableCount > 0 ? TradeActionType.매도 :
            TradeActionType.보합;

        return (tradeCount, tradeActionType);
    }
}
