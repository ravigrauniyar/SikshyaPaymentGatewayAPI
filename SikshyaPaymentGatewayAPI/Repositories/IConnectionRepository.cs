using SikshyaPaymentGatewayAPI.Data;
using SikshyaPaymentGatewayAPI.Models;

namespace SikshyaPaymentGatewayAPI.Repositories
{
    public interface IConnectionRepository
    {
        public string GetDbConnectionString();
        public void ResetDbConnectionString();
        public Task<SikshyaDatabaseContext> UpdateDbContext(DbConnectionModel dbConnectionModel);
    }
}
