namespace SikshyaPaymentGatewayAPI.Services
{
    public interface IPaymentService
    {
        public Task<string> PaymentRequest(EsewaRequestModel model);
        public Task<string> PaymentVerification(EsewaVerificationModel verificationModel, string studentRegistrationModel);
    }
}
