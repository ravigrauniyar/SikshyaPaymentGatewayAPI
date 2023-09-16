using MediatR;

namespace SikshyaPaymentGatewayAPI.Data.Commands
{
    public class ReceiptEntryCommand: IRequest<string>
    {
        public string clientId { get; set; } = string.Empty;
        public string studentRegistrationNumber { get; set; } = string.Empty;
        public double paymentAmount { get; set; }
        public string paymentFrom { get; set; } = string.Empty;
        public ReceiptEntryCommand(string clientId, string studentRegistrationNumber, double paymentAmount, string paymentFrom)
        {
            this.clientId = clientId;
            this.studentRegistrationNumber = studentRegistrationNumber;
            this.paymentAmount = paymentAmount;
            this.paymentFrom = paymentFrom;
        }
    }
}
