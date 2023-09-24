using MediatR;
using SikshyaPaymentGatewayAPI.Data.Commands;
using SikshyaPaymentGatewayAPI.Data.Entities;
using SikshyaPaymentGatewayAPI.Models;
using SikshyaPaymentGatewayAPI.Repositories;

namespace SikshyaPaymentGatewayAPI.Data.Handlers
{
    public class ReceiptEntryHandler: IRequestHandler<ReceiptEntryCommand, ApiResponseModel<OnlinePaymentReceipt>>
    {
        private readonly IPaymentRepository _paymentRepository;
        public ReceiptEntryHandler(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }
        public async Task<ApiResponseModel<OnlinePaymentReceipt>> Handle(ReceiptEntryCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var receiptResponse = await _paymentRepository.AddPaymentReceiptToDB(command.model);
                var apiResponse = receiptResponse.TRANID != string.Empty ?
                                    ApiResponseModel<OnlinePaymentReceipt>.AsSuccess(receiptResponse) :
                                    ApiResponseModel<OnlinePaymentReceipt>.AsFailure("Receipt could not be recorded!");

                return apiResponse;
            } 
            catch (Exception ex)
            {
                var apiResponse = ApiResponseModel<OnlinePaymentReceipt>.AsFailure(ex.Message);
                return apiResponse;
            }
        }
    }
}
