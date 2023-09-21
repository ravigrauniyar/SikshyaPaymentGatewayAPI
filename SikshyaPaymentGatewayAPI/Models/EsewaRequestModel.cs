namespace SikshyaPaymentGatewayAPI
{
    public class EsewaRequestModel
    {
        public string tAmt { get; set; } = string.Empty;
        public string amt { get; set; } = string.Empty;
        public string txAmt { get; set; } = string.Empty;
        public string psc { get; set; } = string.Empty;
        public string pdc { get; set; } = string.Empty;
        public string scd { get; set; } = string.Empty;
        public string pid { get; set; } = string.Empty;
        public string su { get; set; } = string.Empty;
        public string fu { get; set; } = string.Empty;
    }
}