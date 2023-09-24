using SikshyaPaymentGatewayAPI.Data.Entities;
using SikshyaPaymentGatewayAPI.Models;

namespace SikshyaPaymentGatewayAPI.Repositories
{
    public interface IPaymentRepository
    {
        public Task<double> GetStudentBalanceFromDB(GetStudentBalanceModel showBalanceModel);
        public Task<OnlinePaymentReceipt> AddPaymentReceiptToDB(ReceiptEntryModel model);
        public Task<string> AddPaymentNotificationToDB(OnlinePaymentReceipt paymentReceipt, ReceiptEntryModel model);
        public Task<string> AddTransactionToJournal(double amount, string stdRegNo, DbConnectionModel model);
    }
}
