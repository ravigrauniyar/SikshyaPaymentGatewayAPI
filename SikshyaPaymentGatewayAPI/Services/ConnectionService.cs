using SikshyaPaymentGatewayAPI.Models;

namespace SikshyaPaymentGatewayAPI.Services
{
    public class ConnectionService: IConnectionService
    {
        private string _connectionString = string.Empty;

        public string GetConnectionString()
        {
            return _connectionString;
        }

        public void UpdateConnectionString(ConnectionStringModel connectionString)
        {
            _connectionString = $"Server={connectionString.serverIp};Database={connectionString.database};User Id={connectionString.loginId};Password={connectionString.password};";
        }
    }
}
