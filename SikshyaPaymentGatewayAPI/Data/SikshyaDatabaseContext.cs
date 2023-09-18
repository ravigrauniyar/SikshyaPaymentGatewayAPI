using Microsoft.EntityFrameworkCore;
using SikshyaPaymentGatewayAPI.Data.Entities;
using SikshyaPaymentGatewayAPI.Services;

namespace SikshyaPaymentGatewayAPI.Data
{
    public class SikshyaDatabaseContext : DbContext
    {
        private readonly IConnectionService _connectionService;

        public SikshyaDatabaseContext(DbContextOptions options, IConnectionService connectionService) : base(options)
        {
            _connectionService = connectionService;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString = _connectionService.GetConnectionString();

            optionsBuilder.UseSqlServer(connectionString);

            base.OnConfiguring(optionsBuilder);
        }
        public DbSet<TblTrnJournalPartial> TblTrnJournal { get; set; }
        public DbSet<OnlinePaymentReceipt> OnlinePaymentReceipt { get; set;}
        public DbSet<OnlinePayNotification> OnlinePayNotification { get; set;}
    }
}