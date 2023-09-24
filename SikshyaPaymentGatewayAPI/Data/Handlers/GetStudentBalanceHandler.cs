using MediatR;
using SikshyaPaymentGatewayAPI.Data.Queries;
using SikshyaPaymentGatewayAPI.Models;
using SikshyaPaymentGatewayAPI.Repositories;
using SikshyaPaymentGatewayAPI.Utilities;

namespace SikshyaPaymentGatewayAPI.Data.Handlers
{
    public class GetStudentBalanceHandler: IRequestHandler<GetStudentBalanceQuery, ApiResponseModel<double>>
    {
        private readonly IPaymentRepository _paymentRepository;
        public GetStudentBalanceHandler(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }
        public async Task<ApiResponseModel<double>> Handle(GetStudentBalanceQuery query, CancellationToken cancellationToken)
        {
            try
            {
                var balance = await _paymentRepository.GetStudentBalanceFromDB(query.studentBalanceModel);
                var apiResponse = ApiResponseModel<double>.AsSuccess
                (
                    balance, "Balance inquiry successful!"
                );
                return apiResponse;
            }
            catch ( Exception ex )
            {
                var apiResponse = ApiResponseModel<double>.AsFailure(ex.Message);
                return apiResponse;
            }

        }
    }
}
