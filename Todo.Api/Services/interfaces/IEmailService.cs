namespace Todo.Api.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendConfirmationEmail(string toEmail, string emailContent);
    }
}
