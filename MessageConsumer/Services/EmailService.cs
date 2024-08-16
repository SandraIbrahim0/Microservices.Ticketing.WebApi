using Newtonsoft.Json;
using MimeKit;
using MailKit.Net.Smtp;
using Shared;


namespace MessageConsumer.Services
{
    public class EmailService
    {
        public void SendGMail(string message)
        {
            try
            {
                var emailData = JsonConvert.DeserializeObject<EmailModelToParse>(message);
                var email = new MimeMessage();

                email.From.Add(new MailboxAddress("Sender Name", "sandraibrahim365@gamil.com"));
                email.To.Add(new MailboxAddress("Receiver Name", emailData.Recipient));

                email.Subject = "Testing out email sending";
                email.Body = new TextPart(MimeKit.Text.TextFormat.Plain)
                {
                    Text = emailData.Content
                };

                using (var smtp = new SmtpClient())
                {
                    smtp.Connect("smtp.gmail.com", 587, false);

                    // Note: only needed if the SMTP server requires authentication
                    smtp.Authenticate("sandraibrahim365@gmail.com", "wtnl klqe vmrw ywhi");

                    smtp.Send(email);
                    smtp.Disconnect(true);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
            }
        }
            
    }
}
