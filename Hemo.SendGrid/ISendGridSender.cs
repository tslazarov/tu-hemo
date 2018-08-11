using System.Net;

namespace Hemo.SendGrid
{
    public interface ISendGridSender
    {
        HttpStatusCode SendMessage(string fromEmail, string fromName, string toEmail, string subject, string plainTextContent, string htmlContent);

        string GetResetPasswordPlainText(string randomNumber, string language);

        string GetResetPasswordHtml(string randomNumber, string language);
    }
}
