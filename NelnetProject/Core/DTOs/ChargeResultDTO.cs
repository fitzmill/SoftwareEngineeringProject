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
    }
}