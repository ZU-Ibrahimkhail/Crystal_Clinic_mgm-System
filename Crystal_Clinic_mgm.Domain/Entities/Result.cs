namespace Crystal_Clinic_Mgm.Domain.Entities
{
    public class Result
    {
        public bool IsSuccess { get; }
        public string? Error { get; }
        public object? Value { get; }

        protected Result(bool isSuccess, string? error, object? value)
        {
            IsSuccess = isSuccess;
            Error = error;
            Value = value;
        }

        public static Result Success(object? value = null, string? message = null)
        {
            return new Result(true, message, value);
        }

        public static Result Fail(string error)
        {
            return new Result(false, error, null);
        }
    }
}
