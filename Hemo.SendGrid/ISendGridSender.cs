namespace Hemo.SendGrid
{
    public interface ISendGridSender
    {
        void SendMessage(string fromEmail, string fromName, string toEmail, string subject, string plainTextContent, string htmlContent);

        string GetResetPasswordPlainText(string randomNumber);

        string GetResetPasswordHtml(string randomNumber);
    }
}
