using MediatR;
using SikshyaPaymentGatewayAPI.Data.Commands;
using SikshyaPaymentGatewayAPI.Data.Entities;
using SikshyaPaymentGatewayAPI.Repositories;

namespace SikshyaPaymentGatewayAPI.Data.Handlers
{
    public class NotificationEntryHandler: IRequestHandler<NotificationEntryCommand, string>
    {
        private readonly IPaymentRepository _paymentRepository;
        public NotificationEntryHandler(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }
        public async Task<string> Handle(NotificationEntryCommand command, CancellationToken cancellationToken)
        {
            return await _paymentRepository.AddPaymentNotificationToDB(command.paymentReceipt, command.receiptEntry);
        }
    }
}
