
namespace SikshyaPaymentGatewayAPI
{
    public class EsewaInquiryResponseModel
    {
        public string request_id { get; set; } = string.Empty;
        public string response_message { get; set; } = string.Empty;
        public int response_code { get; set; }
        public double amount { get; set; }
        public Dictionary<string, string> properties { get; set; } = new Dictionary<string, string>();
    }
}