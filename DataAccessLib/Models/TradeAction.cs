using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLib.Models
{
    public partial class TradeAction
    {
        public TradeAction() { }
        public byte Id { get; set; }
        public string Name { get; set; } = null!;
    }

    public enum TradeActionType : byte
    {
        None = 0,
        매수 = 1,
        매도 = 2,
        보합 = 3,
    }

    public static class TradeActionTypeExtensions
    {
        public static string GetActionName(this TradeActionType tradeActionType)
        {
            return tradeActionType switch
            {
                TradeActionType.매수 => "매수",
                TradeActionType.매도 => "매도",
                TradeActionType.보합 => "보합",
                _ => throw new InvalidOperationException("tradeActionType의 값이 유효하지 않습니다.")
            };
        }
    }
}
