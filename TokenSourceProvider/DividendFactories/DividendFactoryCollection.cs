using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TokenSourceProvider.DividendFactories
{
    internal class DividendFactoryCollection : IEnumerable<IDividendFactory>
    {
        private IEnumerable<IDividendFactory> _factories;

        public DividendFactoryCollection()
        {
            _factories = [];
        }

        public IEnumerator<IDividendFactory> GetEnumerator()
        {
            return _factories.GetEnumerator();
        }

        internal void AddRange(IDividendFactory[] factories)
        {
            _factories = _factories.Union(factories).ToArray();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
