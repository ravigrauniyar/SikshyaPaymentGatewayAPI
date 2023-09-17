using Microsoft.EntityFrameworkCore;

namespace SikshyaPaymentGatewayAPI.Data.Entities
{
    [Keyless]
    public class TblTrnJournalPartial
    {
        public double DRAMT { get; set; }
        public double CRAMT { get; set; }
    }
}
