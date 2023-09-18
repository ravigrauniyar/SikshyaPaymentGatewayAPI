using MediatR;

namespace SikshyaPaymentGatewayAPI.Data.Commands
{
    public class ReceiptEntryCommand: IRequest<string>
    {
        public string clientId { get; set; } = string.Empty;
        public string studentRegistrationNumber { get; set; } = string.Empty;
        public float paymentAmount { get; set; }
        public string paymentFrom { get; set; } = string.Empty;
        public ReceiptEntryCommand(string clientId, string studentRegistrationNumber, float paymentAmount, string paymentFrom)
        {
            this.clientId = clientId;
            this.studentRegistrationNumber = studentRegistrationNumber;
            this.paymentAmount = paymentAmount;
            this.paymentFrom = paymentFrom;
        }
    }
}
