using MediatR;
using SikshyaPaymentGatewayAPI.Data.Entities;
using SikshyaPaymentGatewayAPI.Models;

namespace SikshyaPaymentGatewayAPI.Data.Commands
{
    public class ReceiptEntryCommand: IRequest<ApiResponseModel<OnlinePaymentReceipt>>
    {
        public ReceiptEntryModel model;
        public ReceiptEntryCommand(ReceiptEntryModel receiptEntryModel) 
        { 
            this.model = receiptEntryModel;
        }
    }
}
