namespace N5PermissionsAPI.Core.Common;

public class Result<T>
{
    public bool IsSuccess { get; }
    public string ErrorMessage { get; }
    public T Value { get; }

    private Result(T value, bool isSuccess, string errorMessage)
    {
        Value = value;
        IsSuccess = isSuccess;
        ErrorMessage = errorMessage;
    }

    public static Result<T> Success(T value) => new Result<T>(value, true, null);
    public static Result<T> Fail(string errorMessage) => new Result<T>(default, false, errorMessage);
}
