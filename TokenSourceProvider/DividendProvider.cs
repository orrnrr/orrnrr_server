using DataAccessLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TokenSourceProvider.DividendFactories;

namespace TokenSourceProvider
{
    internal class DividendProvider
    {
        private static DividendProvider? _instance;
        public static DividendProvider Instance { get => _instance is null ? _instance = new DividendProvider() : _instance; }
        public DividendFactoryCollection Factories { get; } = new DividendFactoryCollection();

        private DividendProvider() { }

        private OrrnrrContext OrrnrrContext { get => OrrnrrContext.Instance; }

        internal void Run()
        {
            var now = DateOnly.FromDateTime(DateTime.Now);

            var query = Factories
                .Where(x => !x.IsPaused)
                .Select(x => x.GetTokenSource());

            var totalTokenSourceIds = OrrnrrContext.TokenSources.Select(x => x.Id);
            var existsDividendTokenSourceIds = OrrnrrContext.DividendHistories
                .Where(x => x.DividendDate >= now)
                .Select(x => x.TokenSourceId);

            var needToDevidendTokenSourceIds = totalTokenSourceIds.Except(existsDividendTokenSourceIds);
#error 배당 지급 로직 마저 작성 필요
        }
    }
}
