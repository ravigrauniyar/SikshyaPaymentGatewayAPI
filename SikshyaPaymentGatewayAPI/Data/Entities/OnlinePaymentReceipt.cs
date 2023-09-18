using Microsoft.EntityFrameworkCore;

namespace SikshyaPaymentGatewayAPI.Data.Entities
{
    [Keyless]
    public class OnlinePaymentReceipt
    {
        public string TRANID { get; set; } = string.Empty;
        public string STREGNO { get; set; } = string.Empty;
        public string PAYFROM { get; set; } = string.Empty;
        public float PAYAMT { get; set; }
        public string TRANEDATE { get; set; } = string.Empty;   
        public string TRANNDATE { get; set; } = string.Empty;
        public string TRNTIME { get; set; } = string.Empty;
        public byte TFLG { get; set; }
        public string RREMARKS { get; set; } = string.Empty;
        public string REFID { get; set; } = string.Empty;
    }
}
