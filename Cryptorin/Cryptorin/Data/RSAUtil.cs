using Cryptorin.Classes.RSAUtil;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Cryptorin.Data
{
    // thank you Zhiqiang Li
    //https://github.com/stulzq/RSAUtil
    //♥ ♥ ♥ ♥ ♥ ♥ ♥ ♥ ♥ ♥ ♥ ♥
    public class RSAUtil
    {
        /// <summary>
        /// Generate RSA key in Pkcs1 format. Result: Index 0 is the private key and index 1 is the public key
        /// </summary>
        /// <returns></returns>
        public List<string> CreateKeys()
        {
            var keyList = RsaKeyGenerator.Pkcs1Key(2048, false);
            //var keyList = RsaKeyGenerator.Pkcs1Key(4096, false);
            //4096 is too heavy for a smartphone, yet
            return keyList;
        }

        /// <summary>
        /// Encrypt message
        /// </summary>
        /// <param name="_publicKey">Public key</param>
        /// <param name="_data">Your message</param>
        /// <returns></returns>
        public string Encrypt(string _publicKey,string _data)
        {
            RsaPkcs1Util bigDataRsa = new RsaPkcs1Util(Encoding.UTF8, _publicKey, null, 2048);
            var str = bigDataRsa.EncryptBigData(_data, RSAEncryptionPadding.Pkcs1);
            return str;
        }

        /// <summary>
        /// Decrypt message
        /// </summary>
        /// <param name="_privateKey">Private key</param>
        /// <param name="_data">Encrypted message</param>
        /// <returns></returns>
        public string Decrypt(string _privateKey, string _data)
        {
            RsaPkcs1Util bigDataRsa2 = new RsaPkcs1Util(Encoding.UTF8, null, _privateKey, 2048);
            return string.Join("", bigDataRsa2.DecryptBigData(_data, RSAEncryptionPadding.Pkcs1));
        }
    }
}
