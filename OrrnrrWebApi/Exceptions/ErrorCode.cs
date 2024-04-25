namespace OrrnrrWebApi.Exceptions
{
    public enum ErrorCode
    {
        None,
        NotFoundUser,
        NotFoundToken,
        InsufficientBalance
    }

    public static class ErrorCodeExtensions
    {
        public static string GetCode(this ErrorCode errorCode)
        {
            return errorCode switch
            {
                ErrorCode.NotFoundUser => "NOT_FOUND_USER",
                ErrorCode.NotFoundToken => "NOT_FOUND_TOKEN",
                ErrorCode.InsufficientBalance => "INSUFFICIENT_BALANCE",
                _ => ""
            };
        }

        public static string GetMessage(this ErrorCode errorCode)
        {
            return errorCode switch
            {
                ErrorCode.NotFoundUser => "유저 정보가 존재하지 않습니다.",
                ErrorCode.NotFoundToken => "토큰 정보가 존재하지 않습니다.",
                ErrorCode.InsufficientBalance => "잔액이 부족합니다.",
                _ => ""
            };
        }
    }
}
