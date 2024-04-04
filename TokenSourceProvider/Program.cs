
using DataAccessLib.Models;
using Newtonsoft.Json;
using TokenSourceProvider.DividendFactories;
using TokenSourceProvider.JsonObjects;

namespace TokenSourceProvider
{
    internal class Program
    {
        internal static OrrnrrContext OrrnrrContext { get; } = OrrnrrContext.Instance;
        static void Main(string[] args)
        {
            DividendProvider provider = DividendProvider.Instance;

            IDividendFactory[] factories = new[]
            {
                new Pm10SeoulDividendFactory(),
            };

            provider.Factories.AddRange(factories);

            provider.Run();

            //await apiTest();
        }

        private static async Task apiTest()
        {
            var tokenSource = OrrnrrContext.TokenSources.FirstOrDefault() ?? throw new NullReferenceException("토큰소스 데이터를 찾을 수 없습니다.");

            var url = tokenSource.RequestUrl;

            var request = new HttpClient();

            using var response = await request.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                throw new InvalidOperationException($"api 요청 결과 실패했습니다. 상태코드: {response.StatusCode}, 메세지: {response.Content}");
            }

            var json = response.Content.ReadAsStringAsync().Result;
            if (json is null)
            {
                throw new InvalidOperationException("api 요청 결과 데이터를 성공적으로 불러오지 못했습니다.");
            }

            Console.WriteLine(json);

            var data = JsonConvert.DeserializeObject<Pm10JsonObj>(json);
            var tempJson = JsonConvert.SerializeObject(data);
            Console.WriteLine(tempJson);
        }
    }
}
