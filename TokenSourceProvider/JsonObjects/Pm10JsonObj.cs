using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TokenSourceProvider.JsonObjects
{
    public record Pm10JsonObj
    {
        [JsonPropertyName("response")]
        public required Pm10Response Response { get; init; }
    }

    public record Pm10Response
    {
        [JsonPropertyName("body")]
        public required Pm10Body Body { get; init; }
        [JsonPropertyName("header")]
        public required Pm10Header Header { get; init; }
    }

    public record Pm10Header
    {
        [JsonPropertyName("resultMsg")]
        public required string ResultMsg { get; init; }
        [JsonPropertyName("resultCode")]
        public required string ResultCode { get; init; }
    }

    public record Pm10Body
    {
        [JsonPropertyName("totalCount")]
        public required int TotalCount { get; init; }
        [JsonPropertyName("items")]
        public required IEnumerable<Pm10Item> Items { get; init; }
        [JsonPropertyName("pageNo")]
        public required int PageNo { get; init; }
        [JsonPropertyName("numOfRows")]
        public required int NumOfRows { get; init; }
    }

    public record Pm10Item
    {
        [JsonPropertyName("daegu")]
        public required string Daegu { get; init; }
        [JsonPropertyName("chungnam")]
        public required string Chungnam { get; init; }
        [JsonPropertyName("incheon")]
        public required string Incheon { get; init; }
        [JsonPropertyName("daejeon")]
        public required string Daejeon { get; init; }
        [JsonPropertyName("gyeongbuk")]
        public required string Gyeongbuk { get; init; }
        [JsonPropertyName("sejong")]
        public required string Sejong { get; init; }
        [JsonPropertyName("gwangju")]
        public required string Gwangju { get; init; }
        [JsonPropertyName("jeonbuk")]
        public required string Jeonbuk { get; init; }
        [JsonPropertyName("gangwon")]
        public required string Gangwon { get; init; }
        [JsonPropertyName("ulsan")]
        public required string Ulsan { get; init; }
        [JsonPropertyName("jeonnam")]
        public required string Jeonnam { get; init; }
        [JsonPropertyName("seoul")]
        public required string Seoul { get; init; }
        [JsonPropertyName("busan")]
        public required string Busan { get; init; }
        [JsonPropertyName("jeju")]
        public required string Jeju { get; init; }
        [JsonPropertyName("chungbuk")]
        public required string Chungbuk { get; init; }
        [JsonPropertyName("gyeongnam")]
        public required string Gyeongnam { get; init; }
        [JsonPropertyName("dataTime")]
        public required string DataTime { get; init; }  // yyyy-MM-dd
        [JsonPropertyName("dataGubun")]
        public required string DataGubun { get; init; }
        [JsonPropertyName("gyeonggi")]
        public required string Gyeonggi { get; init; }
        [JsonPropertyName("itemCode")]
        public required string ItemCode { get; init; }
    }
}
