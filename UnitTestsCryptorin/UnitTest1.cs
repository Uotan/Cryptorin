using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Cryptorin.Data;

namespace UnitTestsCryptorin
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestAESmethod()
        {
            classAES aes = new classAES("4elpsGky8'}|;I[*11111111");
            string crypt = aes.Encrypt("hello");
            string decrypt = aes.Decrypt(crypt);
            bool result = false;
            if (decrypt.Contains("hello"))
            {
                result = true;
            }
            Assert.AreEqual(true, result);
        }


        [TestMethod]
        public void TestRSAmethod()
        {
            classRSA rsa = new classRSA();
            string crypt = rsa.Encrypt("hello");
            string decrypt = rsa.Decrypt(crypt);
            bool result = false;
            if (decrypt.Contains("hello"))
            {
                result = true;
            }
            Assert.AreEqual(true, result);
        }
    }
}
