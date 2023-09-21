using MediatR;
using SikshyaPaymentGatewayAPI.Data.Commands;
using SikshyaPaymentGatewayAPI.Data.Entities;
using SikshyaPaymentGatewayAPI.Repositories;

namespace SikshyaPaymentGatewayAPI.Data.Handlers
{
    public class ReceiptEntryHandler: IRequestHandler<ReceiptEntryCommand, OnlinePaymentReceipt>
    {
        private readonly IPaymentRepository _paymentRepository;
        public ReceiptEntryHandler(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }
        public async Task<OnlinePaymentReceipt> Handle(ReceiptEntryCommand command, CancellationToken cancellationToken)
        {
            return await _paymentRepository.AddPaymentReceiptToDB(command.model);
        }
    }
}
