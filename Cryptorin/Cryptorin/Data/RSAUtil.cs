using Cryptorin.Classes.RSAUtil;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Cryptorin.Data
{
    // thank you Zhiqiang Li
    //https://github.com/stulzq/RSAUtil
    //♡♡♡♡♡♡♡♡♡♡♡♡♡♡♡♡♡♡♡
    public class RSAUtil
    {
        /// <summary>
        /// Generate RSA key in Pkcs1 format. Result: Index 0 is the private key and index 1 is the public key
        /// </summary>
        /// <returns></returns>
        public List<string> CreateKeys()
        {
            var keyList = RsaKeyGenerator.Pkcs1Key(2048, false);
            return keyList;
        }
        public string Encrypt(string _publicKey,string _data)
        {
            RsaPkcs1Util bigDataRsa = new RsaPkcs1Util(Encoding.UTF8, _publicKey, null, 2048);
            var str = bigDataRsa.EncryptBigData(_data, RSAEncryptionPadding.Pkcs1);
            return str;
        }
        public string Decrypt(string _privateKey, string _data)
        {
            RsaPkcs1Util bigDataRsa2 = new RsaPkcs1Util(Encoding.UTF8, null, _privateKey, 2048);
            return string.Join("", bigDataRsa2.DecryptBigData(_data, RSAEncryptionPadding.Pkcs1));
        }
    }
}
