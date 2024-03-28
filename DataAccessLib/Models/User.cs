using System;
using System.Collections.Generic;

namespace DataAccessLib.Models;

public partial class User
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int Balance { get; set; }

    public virtual ICollection<DividendReceiveHistory> DividendReceiveHistories { get; set; } = new List<DividendReceiveHistory>();

    public virtual ICollection<TokenHoldingsHistory> TokenHoldingsHistories { get; set; } = new List<TokenHoldingsHistory>();

    public virtual ICollection<TokenOrderHistory> TokenOrderHistories { get; set; } = new List<TokenOrderHistory>();
}
