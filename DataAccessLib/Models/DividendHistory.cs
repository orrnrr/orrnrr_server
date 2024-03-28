using System;
using System.Collections.Generic;

namespace DataAccessLib.Models;

public partial class DividendHistory
{
    public int Id { get; set; }

    public int TokenSourceId { get; set; }

    public int DividendPerUnit { get; set; }

    public DateOnly DividendDate { get; set; }

    public virtual TokenSource TokenSource { get; set; } = null!;
}
