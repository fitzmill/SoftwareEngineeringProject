using System.Collections.Generic;

namespace Core.DTOs
{
    /// <summary>
    /// Data payload returned from PaymentSpring after charging a user.
    /// </summary>
    public class ChargeResultDTO
    {
        /// <summary>
        /// Whether the charge was successful.
        /// </summary>
        public bool WasSuccessful { get; set; }

        /// <summary>
        /// Error message returned by PaymentSpring, if any.
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// auto-generated overide to the .Equals and .GetHashCode() method to compare these objects
        /// </summary>
        public override bool Equals(object obj)
        {
            var dTO = obj as ChargeResultDTO;
            return dTO != null &&
                   WasSuccessful == dTO.WasSuccessful &&
                   ErrorMessage == dTO.ErrorMessage;
        }

        public override int GetHashCode()
        {
            var hashCode = 1166853200;
            hashCode = hashCode * -1521134295 + WasSuccessful.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(ErrorMessage);
            return hashCode;
        }
    }
}