namespace UsedCarHub.Common.Results
{
    public class Result<T>
    {
        public T Value { get; }
        public bool IsSuccess { get; }
        public string ErrorMessage { get; }

        private Result(T value, bool isSuccess, string errorMessage)
        {
            Value = value;
            IsSuccess = isSuccess;
            ErrorMessage = errorMessage;
        }

        public static Result<T> Success(T value)
        {
            return new Result<T>(value, true, null);
        }

        public static Result<T> Failure(string errorMessage)
        {
            return new Result<T>(default(T), false, errorMessage);
        }
    }
}