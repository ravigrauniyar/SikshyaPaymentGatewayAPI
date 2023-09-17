using Microsoft.EntityFrameworkCore;
using SikshyaPaymentGatewayAPI.Data;
using SikshyaPaymentGatewayAPI.Data.Entities;

namespace SikshyaPaymentGatewayAPI.Repositories
{
    public class PaymentRepository: IPaymentRepository
    {
        private readonly SikshyaDatabaseContext _databaseContext;
        public PaymentRepository(SikshyaDatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<double> GetStudentBalanceFromDB(string clientId, string studentRegistrationNumber)
        {
            var balanceQuery = "Select DRAMT, CRAMT from TblTrnJournal where ACID = '" + studentRegistrationNumber + "' AND VOID = 0";

            var amountList = await _databaseContext
                                .Set<TblTrnJournalPartial>()
                                .FromSqlRaw(balanceQuery)
                                .ToListAsync();

            var studentDrBalance = amountList.Sum(amtItem => amtItem.DRAMT);
            var studentCrBalance = amountList.Sum(amtItem => amtItem.CRAMT);

            return studentDrBalance - studentCrBalance;
        }
        public string AddPaymentReceiptToDB(string clientId, string studentRegistrationNumber, double paymentAmount, string paymentFrom)
        {
            return  "Hi";
        }

    }
}
