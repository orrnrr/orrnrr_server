namespace OrrnrrWebApi.Exceptions
{
    public enum ErrorCode
    {
        None,
        NotFoundUser,
        NotFoundToken
    }

    public static class ErrorCodeExtensions
    {
        public static string GetCode(this ErrorCode errorCode)
        {
            return errorCode switch
            {
                ErrorCode.NotFoundUser => "NOT_FOUND_USER",
                ErrorCode.NotFoundToken => "NOT_FOUND_TOKEN",
                _ => ""
            };
        }

        public static string GetMessage(this ErrorCode errorCode)
        {
            return errorCode switch
            {
                ErrorCode.NotFoundUser => "유저 정보가 존재하지 않습니다.",
                ErrorCode.NotFoundToken => "토큰 정보가 존재하지 않습니다.",
                _ => ""
            };
        }

        public static Error GetError(this ErrorCode errorCode)
        {
            return new Error(errorCode);
        }

        public static Result<TValue> GetResult<TValue>(this ErrorCode errorCode)
        {
            return new Result<TValue>(errorCode);
        }
    }
}
