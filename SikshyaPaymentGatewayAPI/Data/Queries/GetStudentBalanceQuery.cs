using MediatR;

namespace SikshyaPaymentGatewayAPI.Data.Queries
{
    public class GetStudentBalanceQuery: IRequest<double>
    {
        public string clientId { get; set; } = string.Empty;
        public string studentRegistrationNumber { get; set; } = string.Empty;
        public GetStudentBalanceQuery(string clientId, string studentRegistrationNumber)
        {
            this.clientId = clientId;
            this.studentRegistrationNumber = studentRegistrationNumber;
        }
    }
}
