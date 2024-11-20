namespace Todo.Core.Common;

public class Result<T>
{
    public bool IsSuccess { get; }
    public bool IsForbidden { get; }
    public T? Value { get; }
    public string Error { get; }

    private Result(bool isSuccess, bool isForbidden, T? value, string error)
    {
        if (isSuccess && error != string.Empty)
        {
            throw new InvalidOperationException();
        }

        if (!isSuccess && error == string.Empty)
        {
            throw new InvalidOperationException();
        }

        IsSuccess = isSuccess;
        IsForbidden = isForbidden;
        Value = value;
        Error = error;
    }

    public static Result<T> Success(T value)
    {
        return new Result<T>(true, false, value, string.Empty);
    }

    public static Result<T> Failure(string error)
    {
        return new Result<T>(false, false, default, error);
    }

    public static Result<T> Forbidden(string error)
    {
        return new Result<T>(false, true, default, error);
    }
}