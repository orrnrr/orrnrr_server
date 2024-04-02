using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TokenSourceProvider.JsonObjects
{
    public record Pm10JsonObj
    {

        public required Pm10Response response { get; init; }
    }

    public record Pm10Response
    {
        public required Pm10Body body { get; init; }
        public required Pm10Header header { get; init; }
    }

    public record Pm10Header
    {
        public required string resultMsg { get; init; }
        public required string resultCode { get; init; }
    }

    public record Pm10Body
    {
        public required int totalCount { get; init; }
        public required IEnumerable<Pm10Item> items { get; init; }
        public required int pageNo { get; init; }
        public required int numOfRows { get; init; }
    }

    public record Pm10Item
    {
        public required string daegu { get; init; }
        public required string chungnam { get; init; }
        public required string incheon { get; init; }
        public required string daejeon { get; init; }
        public required string gyeongbuk { get; init; }
        public required string sejong { get; init; }
        public required string gwangju { get; init; }
        public required string jeonbuk { get; init; }
        public required string gangwon { get; init; }
        public required string ulsan { get; init; }
        public required string jeonnam { get; init; }
        public required string seoul { get; init; }
        public required string busan { get; init; }
        public required string jeju { get; init; }
        public required string chungbuk { get; init; }
        public required string gyeongnam { get; init; }
        public required string dataTime { get; init; }  // yyyy-MM-dd
        public required string dataGubun { get; init; }
        public required string gyeonggi { get; init; }
        public required string itemCode { get; init; }
    }
}
