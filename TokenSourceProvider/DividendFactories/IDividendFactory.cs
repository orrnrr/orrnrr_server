using DataAccessLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TokenSourceProvider.DividendFactories
{
    internal interface IDividendFactory
    {
        int TokenSourceId { get; }
        bool IsPaused { get; }

        DividendHistory CreateDividendHistory(DateOnly dividendDate);
    }
}
