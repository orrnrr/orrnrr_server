namespace OrrnrrWebApi.Services
{
    public interface ITokenSourceService
    {
        /// <summary>
        /// 주어진 아이디로 해당 토큰소스가 이미 DB에 저장되어 있는지 여부를 반환합니다.
        /// </summary>
        /// <param name="tokenSourceId"></param>
        /// <returns></returns>
        bool IsExistsById(int tokenSourceId);
    }
}
