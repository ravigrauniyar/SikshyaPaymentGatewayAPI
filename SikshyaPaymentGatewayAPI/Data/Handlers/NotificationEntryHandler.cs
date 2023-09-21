using MediatR;
using SikshyaPaymentGatewayAPI.Data.Commands;
using SikshyaPaymentGatewayAPI.Data.Entities;
using SikshyaPaymentGatewayAPI.Repositories;

namespace SikshyaPaymentGatewayAPI.Data.Handlers
{
    public class NotificationEntryHandler: IRequestHandler<NotificationEntryCommand, OnlinePayNotification>
    {
        private readonly IPaymentRepository _paymentRepository;
        public NotificationEntryHandler(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }
        public async Task<OnlinePayNotification> Handle(NotificationEntryCommand command, CancellationToken cancellationToken)
        {
            return await _paymentRepository.AddPaymentNotificationToDB(command.paymentReceipt, command.receiptEntry);
        }
    }
}
