namespace SikshyaPaymentGatewayAPI
{
    public class EsewaRequestModel
    {
        public string tAmt { get; set; } = "100";
        public string amt { get; set; } = "90";
        public string txAmt { get; set; } = "5";
        public string psc { get; set; } = "2";
        public string pdc { get; set; } = "3";
        public string scd { get; set; } = "testmerchant";
        public string pid { get; set; } = "XYZ-1234";
        public string su { get; set; } = "http://abc.com/success.html?q=su";
        public string fu { get; set; } = "http://abc.com/failure.html?q=fu";
    }
}