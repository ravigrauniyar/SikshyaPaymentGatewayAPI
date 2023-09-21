using MediatR;
using SikshyaPaymentGatewayAPI.Data.Entities;
using SikshyaPaymentGatewayAPI.Data.Handlers;
using SikshyaPaymentGatewayAPI.Models;

namespace SikshyaPaymentGatewayAPI.Data.Commands
{
    public class PaymentVerificationCommand: IRequest<string>
    {
        public EsewaVerificationModel verificationModel;
        public GetStudentBalanceModel studentBalanceModel;
        public PaymentVerificationCommand(EsewaVerificationModel model, GetStudentBalanceModel studentBalanceModel)
        {
            verificationModel = model;
            this.studentBalanceModel = studentBalanceModel;
        }
    }
}
