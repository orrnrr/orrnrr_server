using DataAccessLib.Models;
using OrrnrrWebApi.Exceptions;

namespace OrrnrrWebApi.Services
{
    public interface ITransactionService
    {
        /// <summary>
        /// 두 주문을 체결하고 체결기록을 생성합니다.
        /// </summary>
        /// <param name="newOrder"></param>
        /// <param name="matchedOrder"></param>
        /// <param name="signedPrice"></param>
        /// <returns></returns>
        //TransactionHistory CreateTransactionHistory(TokenOrderHistory order1, TokenOrderHistory order2, int signedPrice);
        Result<TransactionHistory> CreateTransactionHistory(TokenOrderHistory order1, TokenOrderHistory order2, int signedPrice);
    }
}
