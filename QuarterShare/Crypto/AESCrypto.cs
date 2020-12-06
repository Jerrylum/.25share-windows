using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace QuarterShare.Crypto
{
    class AESCrypto
    {
        public RijndaelManaged AES { get; }


        public AESCrypto(byte[] Key, byte[] Iv)
        {
            AES = new RijndaelManaged();

            AES.FeedbackSize = 8;
            AES.BlockSize = 128;
            AES.Key = Key;
            AES.IV = Iv;
            AES.Mode = CipherMode.CFB;
            AES.Padding = PaddingMode.None;
        }

        public byte[] ASEEncrypt(byte[] data)
        {
            ICryptoTransform encryptor = AES.CreateEncryptor(AES.Key, AES.IV);

            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    csEncrypt.Write(data, 0, data.Length);
                    return msEncrypt.ToArray();
                }
            }
        }

        public byte[] ASEDecrypt(byte[] data)
        {
            ICryptoTransform decryptor = AES.CreateDecryptor();
            byte[] resultArray= decryptor.TransformFinalBlock(data, 0, data.Length);
            return resultArray;
        }
    }
}
