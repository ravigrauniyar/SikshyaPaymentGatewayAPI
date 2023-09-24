using System.Text.Json.Serialization;

namespace SikshyaPaymentGatewayAPI.Models
{
    public class ApiResponseModel<T>
    {
        public bool isSuccess { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public T data { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? message { get; set; } = null;

        public ApiResponseModel(bool success, T value, string msg) 
        {
            isSuccess = success;
            data = value;
            message = msg;
        }
        public static ApiResponseModel<T> AsSuccess(T value)
        {
            return new ApiResponseModel<T>(true, value, default!);
        }
        public static ApiResponseModel<T> AsSuccess(T value, string msg)
        {
            return new ApiResponseModel<T>(true, value, msg);
        }

        public static ApiResponseModel<T> AsFailure(string msg)
        {
            return new ApiResponseModel<T>(false, default!, msg);
        }
    }
}
