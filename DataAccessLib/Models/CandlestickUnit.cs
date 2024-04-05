using System;
using System.Collections.Generic;

namespace DataAccessLib.Models;

public partial class CandlestickUnit
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    //public virtual ICollection<Candlestick> Candlesticks { get; set; } = new List<Candlestick>();
}
