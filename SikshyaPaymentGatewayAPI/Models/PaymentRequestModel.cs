using System.ComponentModel.DataAnnotations;

namespace SikshyaPaymentGatewayAPI.Models
{
    public class PaymentRequestModel
    {

        // Return URL

        [StringLength(200, ErrorMessage = "RU cannot exceed 200 characters.")]

        public string RU { get; set; } = string.Empty;

        // Merchant code

        [StringLength(20, MinimumLength = 3, ErrorMessage = "PID must have 3 to 20 characters.")]
        public string PID { get; set; } = string.Empty;

        // Product Reference Number

        [StringLength(40, MinimumLength = 3, ErrorMessage = "PRN must have 3 to 40 characters.")]
        public string PRN { get; set; } = string.Empty;

        // Payment Amount

        [StringLength(18, ErrorMessage = "Amount can have at most 18 characters.")]
        public string AMT { get; set; } = string.Empty;

        // Currency

        [StringLength(3, MinimumLength = 3, ErrorMessage = "Currency can only have 3 characters.")]
        public string CRN { get; set; } = "NPR";

        // Date

        [StringLength(10, MinimumLength = 10, ErrorMessage = "Date must have exactly 10 characters.")]
        public string DT { get; set; } = string.Empty;

        // Payment details
        
        [StringLength(160, ErrorMessage = "Payment details (R1) can have at most 160 characters.")]
        public string R1 { get; set; } = string.Empty;

        // Additional Payment details

        [StringLength(50, ErrorMessage = "Additional Payment details (R2) can have at most 50 characters.")]
        public string R2 { get; set; } = string.Empty;

        // Mode eg. P for payment

        [StringLength(3, MinimumLength = 1, ErrorMessage = "Mode of transaction must have 1 to 3 characters.")]
        public string MD { get; set; } = "P";

        // Data validation hash
        public string DV { get; set; } = string.Empty;
    }
}
