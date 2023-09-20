namespace SikshyaPaymentGatewayAPI.Repositories
{
    public interface IPaymentRepository
    {
        public Task<double> GetStudentBalanceFromDB(string studentRegistrationNumber);
        public string AddPaymentReceiptToDB(string clientId, string studentRegistrationNumber, float paymentAmount, string paymentFrom);
        public string AddTransactionToJournal(double amount, string studentRegistrationNumber);
    }
}
