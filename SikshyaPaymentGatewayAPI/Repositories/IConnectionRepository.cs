using SikshyaPaymentGatewayAPI.Data;
using SikshyaPaymentGatewayAPI.Models;

namespace SikshyaPaymentGatewayAPI.Repositories
{
    public interface IConnectionRepository
    {
        public string GetDbConnectionString();
        public Task<SikshyaDatabaseContext> UpdateDbContext(DbConnectionModel dbConnectionModel);
    }
}
