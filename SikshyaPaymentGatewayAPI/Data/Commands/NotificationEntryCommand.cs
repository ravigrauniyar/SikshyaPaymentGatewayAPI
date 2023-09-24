using MediatR;
using SikshyaPaymentGatewayAPI.Data.Entities;
using SikshyaPaymentGatewayAPI.Models;

namespace SikshyaPaymentGatewayAPI.Data.Commands
{
    public class NotificationEntryCommand: IRequest<string>
    {
        public OnlinePaymentReceipt paymentReceipt;
        public ReceiptEntryModel receiptEntry;
        public NotificationEntryCommand(OnlinePaymentReceipt paymentReceipt, ReceiptEntryModel receiptEntry)
        {
            this.paymentReceipt = paymentReceipt;
            this.receiptEntry = receiptEntry;
        }
    }
}
