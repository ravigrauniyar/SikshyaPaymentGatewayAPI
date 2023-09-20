namespace SikshyaPaymentGatewayAPI
{
    public class EsewaVerificationModel
    {
        public string amt { get; set; } = "100";
        public string scd { get; set; } = "testmerchant";
        public string pid { get; set; } = "XYZ-1234";
        public string rid { get; set; } = "000AE01";
    }
}
