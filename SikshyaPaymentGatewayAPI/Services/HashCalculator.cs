using SikshyaPaymentGatewayAPI.Models;
using System;
using System.Security.Cryptography;
using System.Text;

namespace SikshyaPaymentGatewayAPI
{
    public class HashCalculator
    {
        public static string CalculateHmacSha512Hex(string sharedSecret, PaymentRequestModel requestModel)
        {
            try
            {
                // Concatenate relevant properties of the PaymentRequestModel in the specified order
                string concatenatedFields = $"{requestModel.PID},{requestModel.MD},{requestModel.PRN},{requestModel.AMT},{requestModel.CRN},{requestModel.DT},{requestModel.R1},{requestModel.R2},{requestModel.RU}";

                // Convert the concatenated string to UTF-8 encoding
                byte[] dataBytes = Encoding.UTF8.GetBytes(concatenatedFields);

                // Convert the shared secret to bytes
                byte[] keyBytes = Encoding.UTF8.GetBytes(sharedSecret);

                // Create an HMAC-SHA512 hasher using the key
                using var hmacSha512 = new HMACSHA512(keyBytes);

                // Compute the hash
                byte[] hashBytes = hmacSha512.ComputeHash(dataBytes);

                // Convert the hash to a hexadecimal string
                string hexHash = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();

                return hexHash;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
