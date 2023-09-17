namespace SikshyaPaymentGatewayAPI.Repositories
{
    public interface IPaymentRepository
    {
        public Task<double> GetStudentBalanceFromDB(string clientId, string studentRegistrationNumber);
        public string AddPaymentReceiptToDB(string clientId, string studentRegistrationNumber, double paymentAmount, string paymentFrom);
    }
}
