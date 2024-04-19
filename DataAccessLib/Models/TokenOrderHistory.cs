using System;
using System.Collections.Generic;

namespace DataAccessLib.Models;

public partial record TokenOrderHistory
{
    public TokenOrderHistory(User user, Token token, bool isBuyOrder, int price, int count)
    {
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

    public int OrderPrice { get; set; }

    public int OrderCount { get; set; }

    public int CompleteCount { get; set; }

    public DateTime OrderDateTime { get; set; }

    public bool IsCanceled { get; set; }

    public virtual Token Token { get; set; } = null!;

    //public virtual ICollection<TransactionHistory> TransactionHistoryBuyOrders { get; set; } = new List<TransactionHistory>();

    //public virtual ICollection<TransactionHistory> TransactionHistorySellOrders { get; set; } = new List<TransactionHistory>();

    public virtual User User { get; set; } = null!;
    public int ExecutableCount { get => OrderCount - CompleteCount; }

    public static TokenOrderHistory Of(User user, Token token, bool isBuyOrder, int price, int count)
    {
        return new TokenOrderHistory(user, token, isBuyOrder, price, count);
    }

    internal int Trade(TokenOrderHistory other)
    {
        if (IsCanceled || other.IsCanceled)
        {
            throw new InvalidOperationException("거래를 성사시킬 수 없습니다. 취소된 주문이 포함되어 있습니다.");
        }

        if (IsBuyOrder == other.IsBuyOrder)
        {
            throw new InvalidOperationException("거래를 성사시킬 수 없습니다. 두 주문 모두 매수주문이거나 매도주문입니다.");
        }

        int tradeCount = Math.Max(ExecutableCount, other.ExecutableCount);
        if (tradeCount <= 0) {
            throw new InvalidOperationException("거래를 성사시킬 수 없습니다. 두 주문의 거래가능 수량이 0 이하입니다.");
        }

        CompleteCount += tradeCount;
        other.CompleteCount += tradeCount;

        if (ExecutableCount > 0 && other.ExecutableCount > 0)
        {
            throw new InvalidOperationException("거래를 성공적으로 성사시키지 못했습니다.");
        }

        return tradeCount;
    }
}
