using Microsoft.EntityFrameworkCore;

namespace SikshyaPaymentGatewayAPI.Data.Entities
{
    [Keyless]
    public class TblTrnJournalPartial
    {
        public string ACID { get; set; } = string.Empty;
        public int VOID { get; set; }
        public double DRAMT { get; set; }
        public double CRAMT { get; set; }
    }
}
