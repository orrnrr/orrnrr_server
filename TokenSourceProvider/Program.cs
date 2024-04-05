
using DataAccessLib.Models;
using Microsoft.Extensions.Configuration;
using TokenSourceProvider.DividendFactories;

namespace TokenSourceProvider
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true, true)
                .Build();

            ContextManager.Instance.UseOrrnrrContext(config.GetConnectionString("orrnrr") ?? throw new ArgumentNullException("구성 파일에서 orrnrr 연결문자열을 찾을 수 없습니다."));

            DividendProvider provider = DividendProvider.Instance;

            IDividendFactory[] factories = 
            {
                new Pm10SeoulDividendFactory(),
                new Pm10BusanDividendFactory(),
            };

            provider.Factories.AddRange(factories);

            provider.Run();
        }
    }
}
