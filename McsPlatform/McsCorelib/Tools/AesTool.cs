using System.Security.Cryptography;
using System.Text;

namespace McsCoreLib.Tools
{
    public class AesTool
    {
        private static readonly byte[] siv = Encoding.UTF8.GetBytes("McsDesToolNantia");
        private static readonly byte[] mKey = Encoding.UTF8.GetBytes("12345678123456781234567812345678");

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="str">目标值</param>
        /// <param name="skey">密钥</param>
        /// <returns>加密值</returns>
        public static string Encrypt(string str)
        {
            if (string.IsNullOrEmpty(str)) throw new ArgumentNullException(nameof(str));

            try
            {
                byte[] encrypted;

                // Create an Aes object
                // with the specified key and IV.
                using (Aes aesAlg = Aes.Create())
                {
                    aesAlg.Key = mKey;
                    aesAlg.IV = siv;

                    // Create an encryptor to perform the stream transform.
                    ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                    // Create the streams used for encryption.
                    using MemoryStream msEncrypt = new();
                    using CryptoStream csEncrypt = new(msEncrypt, encryptor, CryptoStreamMode.Write);
                    using (StreamWriter swEncrypt = new(csEncrypt))
                    {
                        //Write all data to the stream.
                        swEncrypt.Write(str);
                    }
                    encrypted = msEncrypt.ToArray();
                }
                var x = Convert.ToBase64String(encrypted);
                x ??= "";
                return x;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        ///  解密
        /// </summary>
        /// <param name="str">加密内容</param>

        /// <returns>解密内容</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static string Decrypt(string str)
        {
            if (string.IsNullOrEmpty(str)) throw new ArgumentNullException(nameof(str));

            try
            {
                string plaintext;

                using (Aes aesAlg = Aes.Create())
                {
                    aesAlg.Key = mKey;
                    aesAlg.IV = siv;
                    var d = Convert.FromBase64String(str);
                    // Create a decryptor to perform the stream transform.
                    ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                    // Create the streams used for decryption.
                    using MemoryStream msDecrypt = new(d);
                    using CryptoStream csDecrypt = new(msDecrypt, decryptor, CryptoStreamMode.Read);
                    using StreamReader srDecrypt = new(csDecrypt);
                    plaintext = srDecrypt.ReadToEnd();
                    if (string.IsNullOrEmpty(plaintext)) plaintext = string.Empty;
                }
                return plaintext;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}