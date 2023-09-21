using MediatR;

namespace SikshyaPaymentGatewayAPI.Data.Queries
{
    public class PaymentRequestQuery: IRequest<string>
    {
        public EsewaRequestModel esewaRequestModel;
        public PaymentRequestQuery(EsewaRequestModel esewaRequestModel)
        {
            this.esewaRequestModel = esewaRequestModel;
        }
    }
}
