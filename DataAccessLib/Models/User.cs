using System;
using System.Collections.Generic;

namespace DataAccessLib.Models;

public partial class User
{
    public User() { }
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int Balance { get; set; }

    public bool CanPay(int payment)
    {
        return Balance >= payment;
    }

    internal void PayTo(User other, int payment)
    {
        Balance -= payment;
        other.Balance += payment;

        if (Balance < 0)
        {
            throw new InvalidOperationException("소지금 이상의 금액을 지불할 수 없습니다.");
        }
    }

    //public virtual ICollection<DividendReceiveHistory> DividendReceiveHistories { get; set; } = new List<DividendReceiveHistory>();

    //public virtual ICollection<TokenHoldingsHistory> TokenHoldingsHistories { get; set; } = new List<TokenHoldingsHistory>();

    //public virtual ICollection<TokenOrderHistory> TokenOrderHistories { get; set; } = new List<TokenOrderHistory>();
}
