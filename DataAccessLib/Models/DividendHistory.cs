using System;
using System.Collections.Generic;

namespace DataAccessLib.Models;

public partial class DividendHistory
{
    public int Id { get; init; }

    public int TokenSourceId { get; init; }

    public int DividendPerUnit { get; init; }

    public DateOnly DividendDate { get; init; }

    public virtual TokenSource TokenSource { get; init; } = null!;

    private DividendHistory() { }

    public static DividendHistory Of(int tokenSourceId, int dividendPerUnit, DateOnly dividendDate)
    {
        return new DividendHistory { 
            TokenSourceId = tokenSourceId,
            DividendPerUnit = dividendPerUnit,
            DividendDate = dividendDate
        };
    }
}
