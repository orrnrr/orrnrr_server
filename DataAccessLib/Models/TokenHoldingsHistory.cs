using System;
using System.Collections.Generic;

namespace DataAccessLib.Models;

public partial class TokenHoldingsHistory
{
    public TokenHoldingsHistory() { }

    public TokenHoldingsHistory(User user, Token token)
        : this(user, token, 0, 0.0, 0, 0)
    {
    }

    public TokenHoldingsHistory(User user, Token token, int tokenCount, double averageBuyPrice, int maxBuyPrice, int minBuyPrice)
    {
        UserId = user.Id;
        TokenId = token.Id;
        TokenCount = tokenCount;
        AverageBuyPrice = averageBuyPrice;
        MaxBuyPrice = maxBuyPrice;
        MinBuyPrice = minBuyPrice;
        Token = token;
        User = user;
    }

    public int Id { get; set; }

    public int UserId { get; set; }

    public int TokenId { get; set; }

    public int TokenCount { get; set; }

    public double AverageBuyPrice { get; set; }

    public int MaxBuyPrice { get; set; }

    public int MinBuyPrice { get; set; }

    public DateTime HoldDateTime { get; set; } = DateTime.UtcNow;

    public virtual Token Token { get; set; } = null!;

    public virtual User User { get; set; } = null!;

    public TokenHoldingsHistory Copy()
    {
        var copy = new TokenHoldingsHistory(User, Token, TokenCount, AverageBuyPrice, MaxBuyPrice, MinBuyPrice);

        return copy;
    }
}
