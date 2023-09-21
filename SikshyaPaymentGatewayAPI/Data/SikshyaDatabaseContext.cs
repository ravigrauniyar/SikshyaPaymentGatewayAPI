using Microsoft.EntityFrameworkCore;
using SikshyaPaymentGatewayAPI.Data.Entities;

namespace SikshyaPaymentGatewayAPI.Data
{
    public class SikshyaDatabaseContext : DbContext
    {
        public SikshyaDatabaseContext(DbContextOptions<SikshyaDatabaseContext> options) : base(options)
        {
        }
        public DbSet<DbConnectionCredentials> DbConnectionCredentials { get; set; }
        public DbSet<TblTrnJournalPartial> TblTrnJournal { get; set; }
        public DbSet<OnlinePaymentReceipt> OnlinePaymentReceipt { get; set;}
        public DbSet<OnlinePayNotification> OnlinePayNotification { get; set;}
    }
}