using System.Security.Cryptography;
using System.Text;

namespace McsCoreLib.Tools
{
    public class MD5Encrypt
    {
        public static string Get(string inputValue)
        {
            string cl = inputValue;
            byte[] s = MD5.HashData(Encoding.UTF8.GetBytes(cl));
            return Convert.ToBase64String(s);
        }
    }
}