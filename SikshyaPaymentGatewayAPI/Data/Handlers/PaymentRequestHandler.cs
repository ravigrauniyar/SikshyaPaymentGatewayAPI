using MediatR;
using SikshyaPaymentGatewayAPI.Data.Queries;
using SikshyaPaymentGatewayAPI.Repositories;

namespace SikshyaPaymentGatewayAPI.Data.Handlers
{
    public class PaymentRequestHandler: IRequestHandler<PaymentRequestQuery, string>
    {
        private readonly IConfiguration _configuration;
        private readonly string _baseRequestUrl;
        private readonly string _baseVerifyUrl;
        public PaymentRequestHandler(IConfiguration configuration)
        {
            _configuration = configuration;
            _baseRequestUrl = _configuration.GetValue<string>("esewaDevRequestUrl")!;
            _baseVerifyUrl = _configuration.GetValue<string>("esewaDevVerifyUrl")!;
        }
        public async Task<string> Handle(PaymentRequestQuery query, CancellationToken token)
        {
            var paymentInfo = query.esewaRequestModel;

            var formData = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("tAmt", paymentInfo.tAmt),
                new KeyValuePair<string, string>("amt", paymentInfo.amt),
                new KeyValuePair<string, string>("txAmt", paymentInfo.txAmt),
                new KeyValuePair<string, string>("psc", paymentInfo.psc),
                new KeyValuePair<string, string>("pdc", paymentInfo.pdc),
                new KeyValuePair<string, string>("scd", paymentInfo.scd),
                new KeyValuePair<string, string>("pid", paymentInfo.pid),
                new KeyValuePair<string, string>("su", paymentInfo.su),
                new KeyValuePair<string, string>("fu", paymentInfo.fu)
            });

            using HttpClient httpClient = new();

            // Send the POST request
            HttpResponseMessage response = await httpClient.PostAsync(_baseRequestUrl, formData, token);

            // Check if the request was successful
            if (response.IsSuccessStatusCode)
            {
                // Process the response if needed
                string responseContent = await response.Content.ReadAsStringAsync(token);

                return ("Response Content: " + responseContent);
            }
            else
            {
                return ("Error: " + response.StatusCode);
            }
        }
    }
}
