using Core.Models;

namespace Core.Interfaces.Accessors
{
    /// <summary>
    /// Interface for the accessor to the email sending service.
    /// </summary>
    public interface IEmailAccessor
    {
        /// <summary>
        /// Sends an email notification.
        /// </summary>
        /// <param name="emailNotification">The information needed to send the email</param>
        void SendEmail(EmailNotification emailNotification);
    }
}
