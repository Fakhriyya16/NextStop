
namespace Service.Interfaces
{
    public interface ISendEmail
    {
        Task SendEmailAsync(string to, string subject, string messageBody, bool isHtml = false);
    }
}
