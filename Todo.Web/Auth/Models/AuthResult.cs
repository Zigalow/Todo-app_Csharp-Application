namespace Todo.Web.Auth.Models;

public class AuthResult
{
    public bool Succeeded { get; private set; }
    public string? Error { get; private set; }
    public string? Token { get; private set; }

    private AuthResult(bool succeeded, string? error = null, string? token = null)
    {
        Succeeded = succeeded;
        Error = error;
        Token = token;
    }

    public static AuthResult Success(string? token = null)
    {
        return new AuthResult(true, token: token);
    }

    public static AuthResult Failure(string error)
    {
        return new AuthResult(false, error);
    }

    public static implicit operator bool(AuthResult result)
    {
        return result.Succeeded;
    }
}