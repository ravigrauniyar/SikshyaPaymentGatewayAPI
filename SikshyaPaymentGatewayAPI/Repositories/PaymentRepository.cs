using Microsoft.EntityFrameworkCore;
using SikshyaPaymentGatewayAPI.Data;

namespace SikshyaPaymentGatewayAPI.Repositories
{
    public class PaymentRepository: IPaymentRepository
    {
        private readonly SikshyaDatabaseContext _databaseContext;
        public PaymentRepository(SikshyaDatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public Task<string> AddPaymentReceiptToDB(string clientId, string studentRegistrationNumber, double paymentAmount, string paymentFrom)
        {
            throw new NotImplementedException();
        }

        public Task<double> GetStudentBalanceFromDB(string clientId, string studentRegistrationNumber)
        {
            throw new NotImplementedException();
        }
    }
}
