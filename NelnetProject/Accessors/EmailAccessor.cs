using System;
using System.Net.Mail;
using Core.Exceptions;
using Core.Interfaces;
using Core.Models;

namespace Accessors
{
    /// <summary>
    /// Accessor to email sending service.
    /// </summary>
    public class EmailAccessor : IEmailAccessor
    {

        private static string senderEmail = "efrftgty67hu8j@gmail.com";
        private static string senderPassword = "cornflakes";

        /// <summary>
        /// Sends email notification to external email API.
        /// </summary>
        /// <param name="emailNotification">Notification to be sent</param>
        public void SendEmail(EmailNotification emailNotification)
        {
            if (string.IsNullOrEmpty(emailNotification.To))
            {
                throw new EmailException("Could not send email: No addressee.");
            }
            if (string.IsNullOrEmpty(emailNotification.Subject))
            {
                throw new EmailException("Could not send email: No subject line.");
            }
            if (string.IsNullOrEmpty(emailNotification.Body))
            {
                throw new EmailException("Could not send email: No body.");
            }

            MailMessage email = new MailMessage();
            SmtpClient client = new SmtpClient("smtp.gmail.com");

            email.From = new MailAddress(senderEmail);
            email.To.Add(emailNotification.To);
            email.Subject = emailNotification.Subject;
            email.Body = emailNotification.Body;

            client.Credentials = new System.Net.NetworkCredential(senderEmail, senderPassword);
            client.EnableSsl = true;

            try
            {
                client.Send(email);

            } catch (SmtpException e)
            {
                throw new EmailException("Could not send email: SMTP error", e);
            }
            
        }
    }
}
