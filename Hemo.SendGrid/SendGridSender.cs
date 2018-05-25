using Hemo.SendGrid.Properties;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;

namespace Hemo.SendGrid
{
    public class SendGridSender
    {
        SendGridClient client;

        public SendGridSender()
        {
            this.client = new SendGridClient(Settings.Default.ApiKey);
        }

        public void SendMessage(string fromEmail, string fromName, string toEmail, string subject, string plainTextContent, string htmlContent)
        {
            SendGridMessage message = this.ConstructMessage(fromEmail, fromName, toEmail, subject, plainTextContent, htmlContent);
            this.SendAsync(message).Wait();
        }

        private SendGridMessage ConstructMessage(string fromEmail, string fromName, string toEmail, string subject, string plainTextContent, string htmlContent)
        {
            SendGridMessage message = new SendGridMessage()
            {
                From = new EmailAddress(fromEmail, fromName),
                Subject = subject,
                PlainTextContent = plainTextContent,
                HtmlContent = htmlContent,
            };

            message.AddTo(toEmail);

            return message;
        }

        private async Task SendAsync(SendGridMessage message)
        {
            var response = await this.client.SendEmailAsync(message);

            // TODO: decide how to handle response
        }
    }
}
