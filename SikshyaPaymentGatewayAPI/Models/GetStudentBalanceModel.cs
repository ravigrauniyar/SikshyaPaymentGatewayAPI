namespace SikshyaPaymentGatewayAPI.Models
{
    public class GetStudentBalanceModel
    {
        public int clientId { get; set; }
        public string serverIp { get; set; } = string.Empty;
        public string database { get; set; } = string.Empty;
        public string loginId { get; set; } = string.Empty;
        public string password { get; set; } = string.Empty;
        public string stdRegNo { get; set; } = string.Empty;
    }
}
