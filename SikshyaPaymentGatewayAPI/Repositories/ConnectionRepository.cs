using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SikshyaPaymentGatewayAPI.Data;
using SikshyaPaymentGatewayAPI.Models;

namespace SikshyaPaymentGatewayAPI.Repositories
{
    public class ConnectionRepository : IConnectionRepository
    {
        private readonly IConfiguration _configuration;
        private string _dbConnectionString = string.Empty;
        private readonly DynamicDbContext _dynamicDbContext;
        public ConnectionRepository(IConfiguration configuration, DynamicDbContext dynamicDbContext)
        {
            _configuration = configuration;
            _dynamicDbContext = dynamicDbContext;
        }
        public string GetDbConnectionString()
        {
            return _dbConnectionString != string.Empty ? _dbConnectionString : _configuration.GetConnectionString("MsSqlConnectionString")!;
        }
        public async Task<SikshyaDatabaseContext> UpdateDbContext(DbConnectionModel dbConnectionModel)
        {
            var dbContext = _dynamicDbContext.CreateContext(GetDbConnectionString());

            var dbCredential = await dbContext.DbConnectionCredentials
                                    .FirstOrDefaultAsync
                                    (a =>
                                        a.clientId == dbConnectionModel.clientId &&
                                        a.database == dbConnectionModel.database &&
                                        a.serverIp == dbConnectionModel.serverIp &&
                                        a.loginId == dbConnectionModel.loginId &&
                                        a.password == dbConnectionModel.password
                                    );

            if (dbCredential != null)
            {
                _dbConnectionString = $"Server={dbConnectionModel.serverIp};Database={dbConnectionModel.database};User Id={dbConnectionModel.loginId};Password={dbConnectionModel.password}; TrustServerCertificate=True;";
                return _dynamicDbContext.CreateContext(_dbConnectionString);
            }
            else return dbContext;
        }
    }
}
