namespace SikshyaPaymentGatewayAPI.Models
{
    public class ShowBalanceModel
    {
        public string clientId { get; set; } = string.Empty;
        public string serverIp { get; set; } = string.Empty;
        public string database { get; set; } = string.Empty;
        public string loginId { get; set; } = string.Empty;
        public string password { get; set; } = string.Empty;
        public string studentRegistrationNumber { get; set; } = string.Empty;
    }
}
