using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Core.Interfaces;
using Core.Models;
using Core.Exceptions;

namespace Accessors
{
    /// <summary>
    /// Accessor to email sending service.
    /// </summary>
    public class EmailAccessor : IEmailAccessor
    {

        private string senderEmail;
        private string senderUsername;
        private string senderPassword;
        private int port;

        public EmailAccessor(string senderEmail, string senderUsername, string senderPassword, int port)
        {
            this.senderEmail = senderEmail;
            this.senderUsername = senderUsername;
            this.senderPassword = senderPassword;
            this.port = port;
        }
        
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
            email.IsBodyHtml = true;

            client.Port = port;
            client.Credentials = new System.Net.NetworkCredential(senderUsername, senderPassword);
            client.EnableSsl = true;

            try
            {
                client.Send(email);

            }
            catch (SmtpException e)
            {
                throw new EmailException("Could not send email: SMTP error", e);
            }
        }
    }
}
