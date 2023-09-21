using MediatR;
using SikshyaPaymentGatewayAPI.Models;

namespace SikshyaPaymentGatewayAPI.Data.Queries
{
    public class GetStudentBalanceQuery: IRequest<double>
    {
        public GetStudentBalanceModel model;
        public GetStudentBalanceQuery(GetStudentBalanceModel model)
        {
            this.model = model;
        }
    }
}
