using Core.Interfaces.Accessors;
using Core.Models;
using System;
using System.Diagnostics;
using System.Net.Mail;

namespace Accessors
{
    /// <summary>
    /// Accessor to email sending service.
    /// </summary>
    public class EmailAccessor : IEmailAccessor
    {
        private readonly string _senderEmail;
        private readonly string _senderUsername;
        private readonly string _senderPassword;
        private readonly string _smtpHost;
        private readonly int _port;

        public EmailAccessor(string senderEmail, string senderUsername, string senderPassword, string smtpHost, int port)
        {
            _senderEmail = senderEmail;
            _senderUsername = senderUsername;
            _senderPassword = senderPassword;
            _smtpHost = smtpHost;
            _port = port;
        }
        
        public void SendEmail(EmailNotification emailNotification)
        {
            if (string.IsNullOrEmpty(emailNotification.To))
            {
                throw new ArgumentNullException("'To' field cannot be empty");
            }
            if (string.IsNullOrEmpty(emailNotification.Subject))
            {
                throw new ArgumentNullException("'Subject' field cannot be empty");
            }
            if (string.IsNullOrEmpty(emailNotification.Body))
            {
                throw new ArgumentNullException("'Body' field cannot be empty");
            }

            MailMessage email = new MailMessage();
            SmtpClient client = new SmtpClient(_smtpHost);

            email.From = new MailAddress(_senderEmail);
            email.To.Add(emailNotification.To);
            email.Subject = emailNotification.Subject;
            email.Body = emailNotification.Body;
            email.IsBodyHtml = true;

            client.Port = _port;
            client.Credentials = new System.Net.NetworkCredential(_senderUsername, _senderPassword);
            client.EnableSsl = true;

            client.Send(email);
            Debug.WriteLine("Sent email to " + email.To);
        }
    }
}
