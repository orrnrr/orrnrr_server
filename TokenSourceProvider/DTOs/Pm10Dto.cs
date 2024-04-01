using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TokenSourceProvider.DTOs
{
    internal class Pm10Dto
    {
        //internal required Pm10Response response { get; init; }
        internal required Dictionary<string, Pm10Response> response { get; init; }

        //internal object GetItems()
        //{
        //    response.GetValueOrDefault("response")?.
        //}
    }

    internal class Pm10Response
    {
        //internal required Pm10Body body { get; init; }
        //internal required Pm10Header header { get; init; }

        internal required Dictionary<string, Pm10Body> body { get; init; }
        internal required Dictionary<string, Pm10Header> header { get; init; }
    }

    internal class Pm10Header
    {
        internal required string resultMsg { get; init; }
        internal required string resultCode { get; init; }
    }

    internal class Pm10Body
    {
        internal required int totalCount { get; init; }
        internal required IList<Pm10Item> items { get; init; }
        internal required int pageNo { get; init; }
        internal required int numOfRows { get; init; }
    }

    internal class Pm10Item
    {
        internal required string daegu { get; init; }
        internal required string chungnam { get; init; }
        internal required string incheon { get; init; }
        internal required string daejeon { get; init; }
        internal required string gyeongbuk { get; init; }
        internal required string sejong { get; init; }
        internal required string gwangju { get; init; }
        internal required string jeonbuk { get; init; }
        internal required string gangwon { get; init; }
        internal required string ulsan { get; init; }
        internal required string jeonnam { get; init; }
        internal required string seoul { get; init; }
        internal required string busan { get; init; }
        internal required string jeju { get; init; }
        internal required string chungbuk { get; init; }
        internal required string gyeongnam { get; init; }
        internal required DateTime dataTime { get; init; }
        internal required string dataGubun { get; init; }
        internal required string gyeonggi { get; init; }
        internal required string itemCode { get; init; }
    }
}
