
using DataAccessLib.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Newtonsoft.Json;
using System.Net;
using TokenSourceProvider.DTOs;

namespace TokenSourceProvider
{
    internal class Program
    {
        internal static OrrnrrContext OrrnrrContext { get; } = new OrrnrrContext();
        static async Task Main(string[] args)
        {
            string json = "{\"Property1\":\"Value1\",\"Property2\":{\"NestedProperty1\":\"NestedValue1\",\"NestedProperty2\":{\"DeepProperty1\":\"DeepValue1\",\"DeepProperty2\":\"DeepValue2\"}}}";

            // JSON 문자열을 객체 A로 역직렬화
            var objA = JsonConvert.DeserializeObject<ObjectA>(json);
            var tempJson = JsonConvert.SerializeObject(objA);
            Console.WriteLine(tempJson);
            Console.WriteLine(json);
            Console.WriteLine(json.Equals(tempJson));

            // 역직렬화된 객체 출력
            Console.WriteLine(objA?.Property1); // Output: Value1
            Console.WriteLine(objA?.Property2?.NestedProperty1); // Output: NestedValue1
            Console.WriteLine(objA?.Property2?.NestedProperty2?.DeepProperty1); // Output: DeepValue1
            Console.WriteLine(objA?.Property2?.NestedProperty2?.DeepProperty2); // Output: DeepValue2
        }




        //var tokenSource = OrrnrrContext.TokenSources.FirstOrDefault() ?? throw new NullReferenceException("토큰소스 데이터를 찾을 수 없습니다.");

        //    var url = tokenSource.RequestUrl;

        //    var request = new HttpClient();

        //    using var response = await request.GetAsync(url);
        //    if (!response.IsSuccessStatusCode)
        //    {
        //        throw new InvalidOperationException($"api 요청 결과 실패했습니다. 상태코드: {response.StatusCode}, 메세지: {response.Content}");
        //    }

        //    var json = response.Content.ReadAsStringAsync().Result;
        //    if (json is null)
        //    {
        //        throw new InvalidOperationException("api 요청 결과 데이터를 성공적으로 불러오지 못했습니다.");
        //    }

        //    Console.WriteLine(json);
            
        //    var data = JsonConvert.DeserializeObject<Pm10Dto>(json);
        //    var temp = System.Text.Json.JsonSerializer.Deserialize<Pm10Dto>(json);

        //    Console.WriteLine(JsonConvert.SerializeObject(data));
        //    Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(temp));

        //    var temp1 = data ?? throw new InvalidCastException("data 캐스트 실패");
        //    //var items = data?.response?.body?.items ?? throw new InvalidOperationException("Json을 객체로 제대로 변환하지 못했습니다.");
        //    //var items = data?.GetItems();
        //    //foreach (var item in items)
        //    //{
        //    //    Console.WriteLine(item.seoul);
        //    //}
        //}
    }
    public record ObjectC
    {
        public required string DeepProperty1 { get; set; }
        public required string DeepProperty2 { get; set; }
    }

    public record ObjectB
    {
        public required string NestedProperty1 { get; set; }
        public required ObjectC NestedProperty2 { get; set; }
    }

    public record ObjectA
    {
        public required string Property1 { get; set; }
        public required ObjectB Property2 { get; set; }
    }
}
