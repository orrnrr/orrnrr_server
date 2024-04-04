using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TokenSourceProvider.DividendFactories
{
    internal class Pm10SeoulDividendFactory : IDividendFactory
    {
        public bool IsPaused => false;
    }
}
