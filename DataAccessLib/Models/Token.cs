using System;
using System.Collections.Generic;

namespace DataAccessLib.Models;

public partial class Token
{
    private string _name = null!;
    private string _description = null!;
    public int Id { get; init; }

    public required string Name
    {
        get => _name;
        init
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentOutOfRangeException(nameof(Name), value, "토큰의 이름은 null이거나 비어있을 수 없습니다.");
            }

            _name = value;
        }
    }

    public required string Description
    {
        get => _description;
        init
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentOutOfRangeException(nameof(Description), value, "토큰에 대한 설명은 null이거나 비어있을 수 없습니다.");
            }

            _description = value;
        }
    }

    public int TokenSourceId { get; init; }

    public required virtual TokenSource TokenSource { get; init; }

    //public virtual ICollection<Candlestick> Candlesticks { get; set; } = new List<Candlestick>();

    //public virtual ICollection<DividendReceiveHistory> DividendReceiveHistories { get; set; } = new List<DividendReceiveHistory>();

    //public virtual ICollection<TokenHoldingsHistory> TokenHoldingsHistories { get; set; } = new List<TokenHoldingsHistory>();

    //public virtual ICollection<TokenOrderHistory> TokenOrderHistories { get; set; } = new List<TokenOrderHistory>();
}
