
using OrrnrrWebApi.Exceptions;
using OrrnrrWebApi.Sort;
using System.Text.Json;

namespace OrrnrrWebApi.Types
{
    internal enum OrderType
    {
        None,
        Market, // 시장가
        Limit,  // 지정가
    }

    internal static class OrderTypeExtensions
    {
        public static string[] GetValidValues()
        {
            return Enum.GetValues<OrderType>()
                    .Where(x => x != OrderType.None)
                    .Select(x => x.ToString().ToUpper())
                    .ToArray();
        }

        internal static BadRequestApiException CreateValidRangeApiException(string paramName)
        {
            return new BadRequestApiException($"{paramName}의 값은 다음 중 하나여야 합니다. {JsonSerializer.Serialize(GetValidValues())}");
        }
    }
}
