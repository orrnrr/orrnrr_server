using DataAccessLib.Models;
using OrrnrrWebApi.Types;

namespace OrrnrrWebApi.Services
{
    internal interface IOrdersService
    {
        /// <summary>
        /// 지정가 주문내역을 생성합니다.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="tokenId"></param>
        /// <param name="isBuyOrder"></param>
        /// <param name="price"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        TokenOrderHistory CreateLimitOrder(int userId, int tokenId, bool isBuyOrder, int price, int count);
        /// <summary>
        /// 시장가 주문내역을 생성합니다.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="tokenId"></param>
        /// <param name="isBuyOrder"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        TokenOrderHistory CreateMartetOrder(int userId, int tokenId, bool isBuyOrder, int count);
    }
}
