using DataAccessLib.Models;
using TokenSourceProvider.DividendFactories;

namespace TokenSourceProvider
{
    internal class DividendProvider
    {
        private static DividendProvider? _instance;
        
        public static DividendProvider Instance { get => _instance is null ? _instance = new DividendProvider() : _instance; }
        public DividendFactoryCollection Factories { get; } = new DividendFactoryCollection();
        private OrrnrrContext OrrnrrContext { get => ContextManager.Instance.OrrnrrContext; }
        
        
        private DividendProvider() { }

        internal void Run()
        {
            var yesterday = DateOnly.FromDateTime(DateTime.Now.AddDays(-1));

            var totalTokenSourceIds = OrrnrrContext.TokenSources
                .Select(x => x.Id);

            var existsDividendTokenSourceIds = OrrnrrContext.DividendHistories
                .Where(x => x.DividendDate >= yesterday)
                .Select(x => x.TokenSourceId);

            var needToDevidendTokenSourceIds = totalTokenSourceIds.Except(existsDividendTokenSourceIds);

            var factories = from factory in Factories
                            join id in needToDevidendTokenSourceIds on factory.TokenSourceId equals id
                            where !factory.IsPaused
                            select factory;

            foreach (var factory in factories)
            {
                try
                {
                    var history = factory.CreateDividendHistory(yesterday);
                    if (history is null)
                    {
                        continue;
                    }

                    OrrnrrContext.DividendHistories.AddRange(history);
                }catch
                {
                    continue;
                }
            }

            OrrnrrContext.SaveChanges();
        }
    }
}
