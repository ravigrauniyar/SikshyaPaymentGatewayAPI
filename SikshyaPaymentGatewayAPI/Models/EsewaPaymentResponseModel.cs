
namespace SikshyaPaymentGatewayAPI
{
    public class EsewaPaymentResponseModel
    {
        public string request_id { get; set; } = string.Empty;
        public string response_message { get; set; } = string.Empty;
        public int response_code { get; set; }
        public double amount { get; set; }
        public string reference_id { get; set; } = string.Empty;
    }
}