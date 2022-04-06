using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Cryptorin.Data;
using Cryptorin.Views;
using Cryptorin.Common;

namespace UnitTestsCryptorin
{
    [TestClass]
    public class cryptoTests
    {
        [TestMethod]
        public void TestAESmethod()
        {
            classAES aes = new classAES("4elpsGky8'}|;I[*11111111");
            string crypt = aes.Encrypt("hello");
            string decrypt = aes.Decrypt(crypt);
            decrypt = decrypt.Trim();
            Console.WriteLine(decrypt);
            Assert.AreEqual("hello", decrypt);
        }


        [TestMethod]
        public void TestRSAmethod()
        {
            classRSA rsa = new classRSA();
            string _privateBase = rsa.GetPrivateBase64();
            string _puplicBase = rsa.GetPubliceBase64();
            string crypt = rsa.Encrypt("hello",_puplicBase);
            string decrypt = rsa.Decrypt(crypt,_privateBase);
            decrypt = decrypt.Trim();
            Console.WriteLine(decrypt);
            Assert.AreEqual("hello", decrypt);
        }


        [TestMethod]
        public void TestSHA256()
        {
            classSHA256 rsa = new classSHA256();
            string assert = "2cf24dba5fb0a30e26e83b2ac5b9e29e1b161e5c1fa7425e73043362938b9824";
            string hash = rsa.ComputeSha256Hash("hello");
            Assert.AreEqual(assert, hash);
        }

    }
}
