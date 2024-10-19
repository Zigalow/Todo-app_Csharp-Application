namespace Todo.Core.Common;

public class Result<T>
{
    public bool IsSuccess { get; }
    public T? Value { get; }
    public string Error { get; }

    private Result(bool isSuccess, T? value, string error)
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
        Value = value;
        Error = error;
    }

    public static Result<T> Success(T value)
    {
        return new Result<T>(true, value, string.Empty);
    }

    public static Result<T> Failure(string error)
    {
        return new Result<T>(false, default, error);
    }
}