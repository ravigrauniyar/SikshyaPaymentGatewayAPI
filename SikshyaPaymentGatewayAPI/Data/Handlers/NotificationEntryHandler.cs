using MediatR;
using SikshyaPaymentGatewayAPI.Data.Commands;
using SikshyaPaymentGatewayAPI.Data.Entities;
using SikshyaPaymentGatewayAPI.Models;
using SikshyaPaymentGatewayAPI.Repositories;

namespace SikshyaPaymentGatewayAPI.Data.Handlers
{
    public class NotificationEntryHandler: IRequestHandler<NotificationEntryCommand, ApiResponseModel<string>>
    {
        private readonly IPaymentRepository _paymentRepository;
        public NotificationEntryHandler(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }
        public async Task<ApiResponseModel<string>> Handle(NotificationEntryCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var entryResponse = await _paymentRepository.AddPaymentNotificationToDB(command.paymentReceipt, command.receiptEntry);
                var apiResponse = entryResponse.Contains("Success:") ?
                                    ApiResponseModel<string>.AsSuccess(default!, entryResponse) :
                                    ApiResponseModel<string>.AsFailure(entryResponse);

                return apiResponse;
            }
            catch (Exception ex)
            {
                var apiResponse = ApiResponseModel<string>.AsFailure(ex.Message);
                return apiResponse;
            }
            
        }
    }
}
