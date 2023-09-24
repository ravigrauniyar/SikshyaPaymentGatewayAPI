using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;

namespace SikshyaPaymentGatewayAPI.Utilities
{
    public class CryptographyService
    {
        // Function to encrypt data using AES-256-CBC encryption
        public static string EncryptData<T>(T data, string encryptionKey, string iv)
        {
            using Aes aesAlg = Aes.Create();

            aesAlg.Key = GetValidKey(encryptionKey);
            aesAlg.IV = GetValidIV(iv);

            // Create an encryptor to perform the stream transform.
            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            // Serialize the data object to JSON
            string jsonString = JsonConvert.SerializeObject(data);

            // Create streams for encryption
            using MemoryStream msEncrypt = new();

            using (CryptoStream csEncrypt = new(msEncrypt, encryptor, CryptoStreamMode.Write))
            {
                using StreamWriter swEncrypt = new(csEncrypt);

                // Write the JSON data to the stream.
                swEncrypt.Write(jsonString);
            }

            return Convert.ToBase64String(msEncrypt.ToArray());
        }

        // Function to decrypt data that was previously encrypted using AES-256-CBC
        public static T DecryptData<T>(string encryptedData, string encryptionKey, string iv)
        {
            using Aes aesAlg = Aes.Create();

            aesAlg.Key = GetValidKey(encryptionKey);
            aesAlg.IV = GetValidIV(iv);

            // Create a decryptor to perform the stream transform.
            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            // Convert the Base64 encrypted data back to bytes
            byte[] encryptedBytes = Convert.FromBase64String(encryptedData);

            // Create streams for decryption
            using MemoryStream msDecrypt = new(encryptedBytes);

            using CryptoStream csDecrypt = new(msDecrypt, decryptor, CryptoStreamMode.Read);

            using StreamReader srDecrypt = new(csDecrypt);

            // Read the decrypted bytes from the decrypting stream and deserialize to object
            string decryptedJson = srDecrypt.ReadToEnd();

            return JsonConvert.DeserializeObject<T>(decryptedJson)!;
        }

        public static byte[] GetValidKey(string encryptionKey)
        {
            // Create a SHA-256 hash of the encryption key

            byte[] keyBytes = Encoding.UTF8.GetBytes(encryptionKey);
            byte[] hashBytes = SHA256.HashData(keyBytes);

            // Take the first 32 bytes (256 bits) as the valid key
            byte[] validKey = new byte[32];
            Array.Copy(hashBytes, validKey, 32);

            return validKey;
        }
        public static byte[] GetValidIV(string iv)
        {
            // Ensure IV is exactly 16 bytes long
            byte[] validIV = new byte[16];

            // Copy the bytes from the provided IV to the validIV array
            byte[] ivBytes = Encoding.UTF8.GetBytes(iv);
            Array.Copy(ivBytes, validIV, Math.Min(ivBytes.Length, validIV.Length));

            return validIV;
        }
    }
}
