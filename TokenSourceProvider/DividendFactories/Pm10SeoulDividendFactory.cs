using DataAccessLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TokenSourceProvider.DividendFactories
{
    internal class Pm10SeoulDividendFactory : IDividendFactory
    {
        private const string API_URL = "http://apis.data.go.kr/B552584/ArpltnStatsSvc/getCtprvnMesureLIst?ServiceKey=ko5Hms2xaagEFr0fqTfYkIeDlnWjLQ43xDVAdOYbq6rEuNNW0Ejw%2BI2QkSLiAtGz3QAKtfxQWlP1FdQ%2F6ViMgA%3D%3D&returnType=json&numOfRows=1&pageNo=1&itemCode=pm10&dataGubun=daily&searchCondition=Week";
        public int TokenSourceId => 1;
        public bool IsPaused => false;

        public DividendHistory CreateDividendHistory(DateOnly dividendDate)
        {
#error 배당금액 얻어오는 로직 작성할 것
            //var apiUrl = 


            return DividendHistory.Of(TokenSourceId, 100, dividendDate);
        }
    }
}
