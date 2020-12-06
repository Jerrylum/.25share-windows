using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace QuarterShare.Crypto
{
    class MD5Crypto
    {
        public static string ToMD5(byte[] bytes)
        {
            using (MD5 cryptoMD5 = MD5.Create())
            {
                byte[] hash = cryptoMD5.ComputeHash(bytes);

                string md5 = BitConverter.ToString(hash).Replace("-", string.Empty).ToUpper();

                return md5;
            }
        }
    }
}
