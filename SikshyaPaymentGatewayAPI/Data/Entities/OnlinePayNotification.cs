using Microsoft.EntityFrameworkCore;

namespace SikshyaPaymentGatewayAPI.Data.Entities
{
    [Keyless]
    public class OnlinePayNotification
    {
        public string TRANID { get; set; } = string.Empty;
        public string SCHOOLID { get; set; } = string.Empty;
        public string STREGNO { get; set; } = string.Empty;
        public string EDATE { get; set; } = string.Empty;
        public string TRNTIME { get; set; } = string.Empty;
        public string PAYFROM { get; set; } = string.Empty;
        public byte PAYFLG { get; set; }
        public float PAYAMT { get; set; }
        public string PAYREMARKS { get; set; } = string.Empty;
    }
}
