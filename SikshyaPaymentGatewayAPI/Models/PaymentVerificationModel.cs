using System.ComponentModel.DataAnnotations;

namespace SikshyaPaymentGatewayAPI.Models
{
    public class PaymentVerificationModel
    {
        // Merchant code

        [StringLength(20, MinimumLength = 3, ErrorMessage = "PID must have 3 to 20 characters.")]
        public string PID { get; set; } = string.Empty;

        // Product Reference Number

        [StringLength(40, MinimumLength = 3, ErrorMessage = "PRN must have 3 to 40 characters.")]
        public string PRN { get; set; } = string.Empty;

        // Payment Amount

        [StringLength(18, ErrorMessage = "Amount can have at most 18 characters.")]
        public double AMT { get; set; }

        // Bill ID of payment request
        public string BID { get; set; } = string.Empty;

        // Fone pay Trace ID
        public string UID { get; set; } = string.Empty;

        // Data validation hash
        public string DV { get; set; } = string.Empty;
    }
}
