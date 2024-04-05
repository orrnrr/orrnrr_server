using System;
using System.Collections.Generic;

namespace DataAccessLib.Models;

public partial class TokenSource
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    //public string RequestUrl { get; set; } = null!;

    //public virtual ICollection<DividendHistory> DividendHistories { get; set; } = new List<DividendHistory>();

    //public virtual ICollection<Token> Tokens { get; set; } = new List<Token>();
}
