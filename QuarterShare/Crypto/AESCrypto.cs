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
            AES = new RijndaelManaged
            {
                FeedbackSize = 8,
                BlockSize = 128,
                Key = Key,
                IV = Iv,
                Mode = CipherMode.CFB,
                Padding = PaddingMode.None
            };
        }

        public byte[] ASEEncrypt(byte[] data)
        {
            ICryptoTransform encryptor = AES.CreateEncryptor(AES.Key, AES.IV);

            byte[] resultArray = encryptor.TransformFinalBlock(data, 0, data.Length);
            return resultArray;
        }

        public byte[] ASEDecrypt(byte[] data)
        {
            ICryptoTransform decryptor = AES.CreateDecryptor();
            byte[] resultArray= decryptor.TransformFinalBlock(data, 0, data.Length);
            return resultArray;
        }
    }
}
