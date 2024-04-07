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

            using (var context = OrrnrrContext)
            {
                var aCenturyAgo = DateOnly.FromDateTime(DateTime.Now.AddYears(-100));

                var dividendHistoriesToBeDeleted = context.DividendHistories
                    .Where(x => x.DividendDate < aCenturyAgo);

                context.DividendHistories.RemoveRange(dividendHistoriesToBeDeleted);
                context.SaveChanges();

                var totalTokenSourceIds = context.TokenSources
                    .Select(x => x.Id);

                var existsDividendTokenSourceIds = context.DividendHistories
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

                        context.DividendHistories.AddRange(history);
                    }
                    catch
                    {
                        continue;
                    }
                }

                context.SaveChanges();
            }
        }
    }
}
