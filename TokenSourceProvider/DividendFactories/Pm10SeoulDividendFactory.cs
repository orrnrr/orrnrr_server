using DataAccessLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TokenSourceProvider.JsonObjects;

namespace TokenSourceProvider.DividendFactories
{
    internal class Pm10SeoulDividendFactory : IDividendFactory
    {
        private const string API_URL = "http://apis.data.go.kr/B552584/ArpltnStatsSvc/getCtprvnMesureLIst?ServiceKey=ko5Hms2xaagEFr0fqTfYkIeDlnWjLQ43xDVAdOYbq6rEuNNW0Ejw%2BI2QkSLiAtGz3QAKtfxQWlP1FdQ%2F6ViMgA%3D%3D&returnType=json&numOfRows=10&pageNo=1&itemCode=pm10&dataGubun=daily&searchCondition=Week";
        public int TokenSourceId => 1;
        public bool IsPaused => false;

        public DividendHistory? CreateDividendHistory(DateOnly dividendDate)
        {
            try
            {
                var request = new HttpClient();
                using var response = request.GetAsync(API_URL).Result;
                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                var json = response.Content.ReadAsStringAsync().Result;
                if (string.IsNullOrEmpty(json))
                {
                    return null;
                }

                var data = System.Text.Json.JsonSerializer.Deserialize<Pm10JsonObj>(json);
                if (data is null)
                {
                    return null;
                }

                var seoul = (from item in data.Response.Body.Items
                            where item.DataTime.Equals(dividendDate.ToString("yyyy-MM-dd"), StringComparison.OrdinalIgnoreCase)
                            select item.Seoul)
                            .First();

                return DividendHistory.Of(TokenSourceId, int.Parse(seoul), dividendDate);
            }
            catch
            {
                return null;
            }
        }
    }
}
