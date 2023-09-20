using SikshyaPaymentGatewayAPI.Models;
using System.Web;

namespace SikshyaPaymentGatewayAPI.Services
{
    public class UrlBuilder
    {
        public static string BuildRequestUrl(string baseUrl, PaymentRequestModel requestModel)
        {
            // Create a dictionary for query parameters
            var queryParams = new Dictionary<string, string>
            {
                { "PID", requestModel.PID },
                { "MD", requestModel.MD },
                { "AMT", requestModel.AMT },
                { "CRN", requestModel.CRN },
                { "DT", requestModel.DT },
                { "R1", requestModel.R1 },
                { "R2", requestModel.R2 },
                { "DV", requestModel.DV },
                { "RU", requestModel.RU },
                { "PRN", requestModel.PRN }
            };

            // Create a UriBuilder to construct the URL
            var uriBuilder = new UriBuilder(baseUrl);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);

            foreach (var param in queryParams)
            {
                query[param.Key] = param.Value;
            }

            // Set the updated query back to the UriBuilder
            uriBuilder.Query = query.ToString();

            // Get the final URL as a string
            string requestUrl = uriBuilder.Uri.ToString();

            return requestUrl;
        }
    }
}
