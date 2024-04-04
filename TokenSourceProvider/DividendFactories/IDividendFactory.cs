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
        bool IsPaused { get; }

        TokenSource GetTokenSource();
    }
}
