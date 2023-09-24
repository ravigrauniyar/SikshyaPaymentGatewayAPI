
namespace SikshyaPaymentGatewayAPI
{
    public class EsewaFailureResponseModel
    {
        public string request_id { get; set; } = string.Empty;
        public int response_code { get; set; }
    }
}