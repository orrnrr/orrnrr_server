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
}
