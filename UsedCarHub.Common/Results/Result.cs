using UsedCarHub.Common.Errors;

namespace UsedCarHub.Common.Results
{
    public class Result<T>
    {
        public T Value { get; }
        public bool IsSuccess { get; }
        public Error ExecutionError { get; }

        private Result(T value, bool isSuccess, Error executionError)
        {
            Value = value;
            IsSuccess = isSuccess;
            ExecutionError = executionError;
        }

        public static Result<T> Success(T value)
        {
            return new Result<T>(value, true, null);
        }

        public static Result<T> Failure(Error executionError)
        {
            return new Result<T>(default(T), false, executionError);
        }
    }
}