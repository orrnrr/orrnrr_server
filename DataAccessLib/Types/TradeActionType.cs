using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLib.Types
{
    public enum TradeActionType : byte
    {
        None = 0,
        Buy = 1,
        Sell = 2,
        Hold = 3,
    }
}
