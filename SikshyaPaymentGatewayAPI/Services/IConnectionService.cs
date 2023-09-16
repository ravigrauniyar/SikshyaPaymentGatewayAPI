using SikshyaPaymentGatewayAPI.Models;

namespace SikshyaPaymentGatewayAPI.Services
{
    public interface IConnectionService
    {
        public string GetConnectionString();
        public void UpdateConnectionString(ConnectionStringModel connectionString);
    }
}
