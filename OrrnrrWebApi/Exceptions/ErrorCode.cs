namespace OrrnrrWebApi.Exceptions
{
    public enum ErrorCode
    {
        None,
        NotFoundUser,
        NotFoundToken,
        HaveNotEnoughBalance,
        HaveNotEnoughToken
    }

    public static class ErrorCodeExtensions
    {
        public static string GetCode(this ErrorCode errorCode)
        {
            return errorCode switch
            {
                ErrorCode.NotFoundUser => "NOT_FOUND_USER",
                ErrorCode.NotFoundToken => "NOT_FOUND_TOKEN",
                ErrorCode.HaveNotEnoughBalance => "HAVE_NOT_ENOUGH_BALANCE",
                ErrorCode.HaveNotEnoughToken => "HAVE_NOT_ENOUGH_TOKEN",
                _ => ""
            };
        }

        public static string GetMessage(this ErrorCode errorCode)
        {
            return errorCode switch
            {
                ErrorCode.NotFoundUser => "유저 정보가 존재하지 않습니다.",
                ErrorCode.NotFoundToken => "토큰 정보가 존재하지 않습니다.",
                ErrorCode.HaveNotEnoughBalance => "잔액이 부족합니다.",
                ErrorCode.HaveNotEnoughToken => "보유한 토큰의 수량이 부족합니다.",
                _ => ""
            };
        }
    }
}
