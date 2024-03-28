using System;
using System.Collections.Generic;

namespace DataAccessLib.Models;

public partial class TokenHoldingsHistory
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int TokenId { get; set; }

    public int TokenCount { get; set; }

    public double AverageBuyPrice { get; set; }

    public int MaxBuyPrice { get; set; }

    public int MinBuyPrice { get; set; }

    public DateOnly HoldDate { get; set; }

    public virtual Token Token { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
