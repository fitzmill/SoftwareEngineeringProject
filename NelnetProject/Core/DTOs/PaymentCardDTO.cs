using System;
using System.Collections.Generic;
using System.Text;

namespace Core.DTOs
{
    public class PaymentCardDTO
    {
        public string CustomerID { get; set; }

        public long CardNumber { get; set; }

        public int ExpirationYear { get; set; }

        public int ExpirationMonth { get; set; }

        //auto-generated overide to the .Equals and .GetHashCode method to compare these objects
        public override bool Equals(object obj)
        {
            var dTO = obj as PaymentCardDTO;
            return dTO != null &&
                   CustomerID == dTO.CustomerID &&
                   CardNumber == dTO.CardNumber &&
                   ExpirationYear == dTO.ExpirationYear &&
                   ExpirationMonth == dTO.ExpirationMonth;
        }

        public override int GetHashCode()
        {
            var hashCode = 920087689;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(CustomerID);
            hashCode = hashCode * -1521134295 + CardNumber.GetHashCode();
            hashCode = hashCode * -1521134295 + ExpirationYear.GetHashCode();
            hashCode = hashCode * -1521134295 + ExpirationMonth.GetHashCode();
            return hashCode;
        }
    }
}
