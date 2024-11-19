namespace Todo.Web.Auth.Models;

public class UserInfoRequest
{
    public int id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string? PhoneNumber { get; set; }
}