using System;
using System.Collections.Generic;

namespace DataAccessLib.Models;

public partial class DividendReceiveHistory
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int TokenId { get; set; }

    public int DividendAmount { get; set; }

    public DateTime ReceiveDateTime { get; set; }

    public virtual Token Token { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
