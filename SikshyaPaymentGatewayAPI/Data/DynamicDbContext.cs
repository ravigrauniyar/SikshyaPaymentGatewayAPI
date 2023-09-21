using Microsoft.EntityFrameworkCore;
using SikshyaPaymentGatewayAPI.Repositories;

namespace SikshyaPaymentGatewayAPI.Data
{
    public class DynamicDbContext
    {
        private readonly DbContextOptionsBuilder<SikshyaDatabaseContext> _optionsBuilder;
        public DynamicDbContext()
        {
            _optionsBuilder = new DbContextOptionsBuilder<SikshyaDatabaseContext>();
        }
        public SikshyaDatabaseContext CreateContext(string connectionString)
        {
            // Set the connection string dynamically
            _optionsBuilder.UseSqlServer(connectionString);

            var dbContextOptions = _optionsBuilder.Options;

            return new SikshyaDatabaseContext(dbContextOptions);
        }
    }

}
