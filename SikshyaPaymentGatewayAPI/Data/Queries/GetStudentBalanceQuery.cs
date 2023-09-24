using MediatR;
using SikshyaPaymentGatewayAPI.Models;

namespace SikshyaPaymentGatewayAPI.Data.Queries
{
    public class GetStudentBalanceQuery: IRequest<double>
    {
        public GetStudentBalanceModel studentBalanceModel;
        public GetStudentBalanceQuery(GetStudentBalanceModel studentBalanceModel)
        {
            this.studentBalanceModel = studentBalanceModel;
        }
    }
}
