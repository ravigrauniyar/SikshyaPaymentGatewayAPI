using MediatR;
using SikshyaPaymentGatewayAPI.Data.Queries;
using SikshyaPaymentGatewayAPI.Models;
using SikshyaPaymentGatewayAPI.Repositories;
using SikshyaPaymentGatewayAPI.Utilities;

namespace SikshyaPaymentGatewayAPI.Data.Handlers
{
    public class GetStudentBalanceHandler: IRequestHandler<GetStudentBalanceQuery, double>
    {
        private readonly IPaymentRepository _paymentRepository;
        public GetStudentBalanceHandler(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }
        public async Task<double> Handle(GetStudentBalanceQuery query, CancellationToken cancellationToken)
        {
            return await _paymentRepository.GetStudentBalanceFromDB(query.studentBalanceModel);
        }
    }
}
