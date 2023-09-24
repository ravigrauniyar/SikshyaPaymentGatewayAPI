using MediatR;
using SikshyaPaymentGatewayAPI.Data.Commands;
using SikshyaPaymentGatewayAPI.Repositories;
using System.Xml;

namespace SikshyaPaymentGatewayAPI.Data.Handlers
{
    public class PaymentVerificationHandler: IRequestHandler<PaymentVerificationCommand, string>
    {
        private readonly IConfiguration _configuration;
        private readonly IPaymentRepository _paymentRepository;
        private readonly string _baseVerifyUrl;
        public PaymentVerificationHandler(IConfiguration configuration, IPaymentRepository paymentRepository)
        {
            _configuration = configuration;
            _paymentRepository = paymentRepository;
            _baseVerifyUrl = _configuration.GetValue<string>("esewaDevVerifyUrl")!;
        }
        public async Task<string> Handle(PaymentVerificationCommand command, CancellationToken token)
        {
            var verificationModel = command.verificationModel;

            using HttpClient httpClient = new();

            // Construct the query string with the model parameters
            string queryString = $"amt={verificationModel.amt}&scd={verificationModel.scd}&pid={verificationModel.pid}&rid={verificationModel.rid}";

            // Combine the base URL with the query string
            string url = $"{_baseVerifyUrl}?{queryString}";

            // Send the GET request
            HttpResponseMessage response = await httpClient.GetAsync(url, token);


            // Check if the request was successful
            if (response.IsSuccessStatusCode)
            {
                string responseContent = await response.Content.ReadAsStringAsync(token);

                XmlDocument xmlDoc = new();

                xmlDoc.LoadXml(responseContent);

                XmlNode responseCodeNode = xmlDoc.SelectSingleNode("//response_code")!;
                return responseCodeNode.InnerText;
                /*if (responseCodeNode != null)
                {
                    string responseCode = responseCodeNode.InnerText;

                    // Check if responseCode is Success then proceed

                    var amt = Convert.ToDouble(verificationModel.amt);

                    return await _paymentRepository.AddTransactionToJournal(amt, command.studentBalanceModel);
                }
                else
                {
                    return ("Response Code not found in XML.");
                }
            */}
            else
            {
                return ("Error: " + response.StatusCode);
            }
        }
    }
}
