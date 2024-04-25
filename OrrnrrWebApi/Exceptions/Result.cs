namespace OrrnrrWebApi.Exceptions
{
    public class Result<TValue>
    {
        private readonly TValue? _value;
        private readonly Error? _error;

        public Result(TValue value) {
            _value = value;
        }

        public Result(string message, ErrorCode errorCode)
        {
            _error = new Error(errorCode, message);
        }

        public Result(ErrorCode errorCode)
        {
            _error = errorCode.GetError();
        }

        public Result(Error error)
        {
            _error = error;
        }

        public TValue Value { get => _value ?? throw new NullReferenceException(); }
        public Error Error { get => _error ?? throw new NullReferenceException(); }
        public bool IsSuccess { get => _value != null; }

    }

    public class Error
    {
        public Error(ErrorCode errorCode, string message)
        {
            Message = message;
            Code = errorCode.GetCode();
        }
        public Error(ErrorCode code)
        {
            Message = code.GetMessage();
            Code = code.GetCode();
        }

        public string Message { get; }
        public string Code { get; }
    }
}
