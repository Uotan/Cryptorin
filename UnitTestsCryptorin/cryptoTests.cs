using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Cryptorin.Data;
using Cryptorin.Views;
using Cryptorin.Common;
using System.Net;

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
            string _puplicBase = rsa.GetPublicBase64();
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

        [TestMethod]
        public void TestSendMessage()
        {
            classSignature signature = new classSignature();
            publicUserData result = signature.SignIn("user", "a665a45920422f9d417e4867efdc4fb8a04a1f3fff1fa07e998e86f7f7a27ae3");
            //string result = signature.SignUp("QwertyUiop", "qwerty12", "a665a45920422f9d417e4867efdc4fb8a04a1f3fff1fa07e998e86f7f7a27ae3", null);
            Console.WriteLine(result.public_name);
            Assert.AreEqual(1, result.id);
        }

    }
}
