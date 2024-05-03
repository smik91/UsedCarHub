using UsedCarHub.Common.Errors;

namespace UsedCarHub.Common.Results
{
    public class Result<T>
    {
        public T Value { get; }
        public bool IsSuccess { get; }
        public IReadOnlyList<Error> ExecutionErrors { get; }

        private Result(T value, bool isSuccess, IReadOnlyList<Error> errors)
        {
            Value = value;
            IsSuccess = isSuccess;
            ExecutionErrors = errors;
        }

        public static Result<T> Success(T value)
        {
            return new Result<T>(value, true, null);
        }

        public static Result<T> Failure(Error error)
        {
            return new Result<T>(default(T), false, new List<Error> { error });
        }
        
        public static Result<T> Failure(IEnumerable<Error> errors)
        {
            return new Result<T>(default(T), false, errors.ToList());
        }
    }
}