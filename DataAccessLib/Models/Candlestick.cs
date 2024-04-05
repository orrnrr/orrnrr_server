using System;
using System.Collections.Generic;

namespace DataAccessLib.Models;

public partial class Candlestick
{
    public int Id { get; set; }

    public int CandlestickUnitId { get; set; }

    public int TokenId { get; set; }

    public int BeginPrice { get; set; }

    public int MaxPrice { get; set; }

    public int MinPrice { get; set; }

    public int EndPrice { get; set; }

    public int TransactionVolume { get; set; }

    public DateTime BeginDateTime { get; set; }

    public virtual CandlestickUnit CandlestickUnit { get; set; } = null!;

    public virtual Token Token { get; set; } = null!;
}
