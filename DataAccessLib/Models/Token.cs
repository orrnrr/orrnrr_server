using System;
using System.Collections.Generic;

namespace DataAccessLib.Models;

public partial class Token
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public int TokenSourceId { get; set; }

    public virtual ICollection<Candlestick> Candlesticks { get; set; } = new List<Candlestick>();

    public virtual ICollection<DividendReceiveHistory> DividendReceiveHistories { get; set; } = new List<DividendReceiveHistory>();

    public virtual ICollection<TokenHoldingsHistory> TokenHoldingsHistories { get; set; } = new List<TokenHoldingsHistory>();

    public virtual ICollection<TokenOrderHistory> TokenOrderHistories { get; set; } = new List<TokenOrderHistory>();

    public virtual TokenSource TokenSource { get; set; } = null!;
}
