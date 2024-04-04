using DataAccessLib.Models;
using Newtonsoft.Json;
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
        private OrrnrrContext OrrnrrContext { get => OrrnrrContext.Instance; }
        
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

            var dividendHistories = from factory in Factories
                        join id in needToDevidendTokenSourceIds on factory.TokenSourceId equals id
                        where !factory.IsPaused
                        select factory.CreateDividendHistory(yesterday);

            OrrnrrContext.DividendHistories.AddRange(dividendHistories);




            foreach(var history in dividendHistories)
            {
                Console.WriteLine(JsonConvert.SerializeObject(history));
                Console.WriteLine();
            }
        }
    }
}
