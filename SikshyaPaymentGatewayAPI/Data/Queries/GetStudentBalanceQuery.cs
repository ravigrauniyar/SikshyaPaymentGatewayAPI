using MediatR;
using SikshyaPaymentGatewayAPI.Models;

namespace SikshyaPaymentGatewayAPI.Data.Queries
{
    public class GetStudentBalanceQuery: IRequest<ApiResponseModel<double>>
    {
        public GetStudentBalanceModel studentBalanceModel;
        public GetStudentBalanceQuery(GetStudentBalanceModel studentBalanceModel)
        {
            this.studentBalanceModel = studentBalanceModel;
        }
    }
}
