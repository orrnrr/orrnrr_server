﻿using DataAccessLib.Models;
using OrrnrrWebApi.Pagination;
using OrrnrrWebApi.Responses;
using OrrnrrWebApi.Sort;

namespace OrrnrrWebApi.Services
{
    public interface ITokenService
    {
        /// <summary>
        /// 주어진 인자들로 토큰을 생성하고 생성된 토큰을 반환합니다. 생성에 실패하면 에러를 뿜뿜합니다.
        /// </summary>
        /// <param name="tokenSourceId"></param>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        Token CreateToken(int tokenSourceId, string name, string description);

        /// <summary>
        /// 주어진 토큰 아이디로 토큰을 조회합니다. 토큰이 없는 경우 null을 반환합니다.
        /// </summary>
        /// <param name="tokenId"></param>
        /// <returns></returns>
        Token? GetTokenById(int tokenId);
        PaginationResponse<TokenResponse> GetTokensOrderByChangeRate(Paging paging, Ordering ordering);
        PaginationResponse<TokenResponse> GetTokensOrderByCurrentPrice(Paging paging, Ordering ordering);
        PaginationResponse<TokenResponse> GetTokensOrderByTradeAmount(Paging paging, Ordering ordering);
        /// <summary>
        /// 이미 같은 이름의 토큰이 존재하면 true, 아니면 false를 반환합니다.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        bool IsExistsTokenName(string name);
    }
}