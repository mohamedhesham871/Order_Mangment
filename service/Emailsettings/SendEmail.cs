using System.Net.Mail;
using System.Net;
using Stripe.Climate;

namespace Api.Emailsettings
{
    public static class SendEmail
    {
        public static async Task SendingMessage(string ToEmail,int OrderID,string OrderStatus)
        {
           
            var EmailMessage = new MailMessage();
            EmailMessage.From = new MailAddress("mmmelkady23@gmail.com");
            EmailMessage.To.Add(ToEmail);
            EmailMessage.Subject = $"Your Order :{OrderID} ";
            EmailMessage.Body = $"Your Order With Id: {OrderID}  Now is {OrderStatus } , We Hope be Pleasure ...";

            using var SMTP = new SmtpClient(host: "smtp.gmail.com", port: 587);
            SMTP.EnableSsl = true;
            SMTP.Credentials = new NetworkCredential(userName: "mmmelkady23@gmail.com", "qhidnzdqvuizsiyn");
            try
            {
                await SMTP.SendMailAsync(EmailMessage);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to send  email.", ex);
            }
            finally
            {
                EmailMessage.Dispose(); // Dispose of the email message to free resources
            }
        }
    }
}

