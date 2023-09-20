using HtmlAgilityPack;
using SikshyaPaymentGatewayAPI.Models;
using SikshyaPaymentGatewayAPI.Repositories;
using System.Net.Http;
using System.Web;
using System.Xml;

namespace SikshyaPaymentGatewayAPI.Services
{
    public class PaymentService: IPaymentService
    {
        private readonly IConfiguration _configuration;
        private readonly IPaymentRepository _paymentRepository;
        private readonly string _baseRequestUrl;
        private readonly string _baseVerifyUrl;
        public PaymentService(IConfiguration configuration, IPaymentRepository paymentRepository)
        {
            _configuration = configuration;
            _baseRequestUrl = _configuration.GetValue<string>("esewaDevRequestUrl")!;
            _baseVerifyUrl = _configuration.GetValue<string>("esewaDevVerifyUrl")!;
            _paymentRepository = paymentRepository;
        }

        public async Task<string> PaymentRequest( EsewaRequestModel paymentInfo)
        {
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
            HttpResponseMessage response = await httpClient.PostAsync(_baseRequestUrl, formData);

            // Check if the request was successful
            if (response.IsSuccessStatusCode)
            {
                // Process the response if needed
                string responseContent = await response.Content.ReadAsStringAsync();
                return ("Response Content: " + responseContent);
            }
            else
            {
                return ("Error: " + response.StatusCode);
            }
        }

        public async Task<string> PaymentVerification(EsewaVerificationModel verificationModel, string studentRegistrationNumber)
        {
            using HttpClient httpClient = new();

            // Construct the query string with the model parameters
            string queryString = $"amt={verificationModel.amt}&scd={verificationModel.scd}&pid={verificationModel.pid}&rid={verificationModel.rid}";

            // Combine the base URL with the query string
            string url = $"{_baseVerifyUrl}?{queryString}";

            // Send the GET request
            HttpResponseMessage response = await httpClient.GetAsync(url);


            // Check if the request was successful
            if (response.IsSuccessStatusCode)
            {
                string responseContent = await response.Content.ReadAsStringAsync();

                XmlDocument xmlDoc = new();

                xmlDoc.LoadXml(responseContent);

                XmlNode responseCodeNode = xmlDoc.SelectSingleNode("//response_code")!;

                if (responseCodeNode != null)
                {
                    string responseCode = responseCodeNode.InnerText;
                    return _paymentRepository.AddTransactionToJournal(Convert.ToDouble(verificationModel.amt), studentRegistrationNumber);
                }
                else
                {
                    return ("Response Code not found in XML.");
                }
            }
            else
            {
                return ("Error: " + response.StatusCode);
            }
        }
    }
}
