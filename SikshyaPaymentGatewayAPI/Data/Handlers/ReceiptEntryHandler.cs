using MediatR;
using SikshyaPaymentGatewayAPI.Data.Commands;
using SikshyaPaymentGatewayAPI.Repositories;

namespace SikshyaPaymentGatewayAPI.Data.Handlers
{
    public class ReceiptEntryHandler: IRequestHandler<ReceiptEntryCommand, string>
    {
        private readonly IPaymentRepository _paymentRepository;
        public ReceiptEntryHandler(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }
        public async Task<string> Handle(ReceiptEntryCommand command, CancellationToken cancellationToken)
        {
            return await _paymentRepository.AddPaymentReceiptToDB(command.clientId, command.studentRegistrationNumber, command.paymentAmount, command.paymentFrom);
        }
    }
}
