using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Cryptorin.Data;
using Cryptorin.Views;
using Cryptorin.Common;
using Cryptorin.Classes;

namespace UnitTestsCryptorin
{
    [TestClass]
    public class signTests
    {
        [TestMethod]
        public void TestLoginMatch()
        {
            classSignature signature = new classSignature();
            string result = signature.CheckLoginExists("user");
            Assert.AreEqual("exists", result);
        }


        [TestMethod]
        public void TestSignUp()
        {
            classSignature signature = new classSignature();
            classSHA256 SHA = new classSHA256();
            string password = SHA.ComputeSha256Hash("123");
            string result = signature.SignUp("Unit","unit", password, "iVBORw0KGgoAAAANSUhEUgAAAEAAAABACAYAAACqaXHeAAAAAXNSR0IArs4c6QAAAZJJREFUeJztmkFuwkAMRT9VbwGnaHft+VlyC+4BK6R0VMJkxvYz4L9DQuT7xfbYCbv9189Fb6wP2gCtAkAboFUAaAO0CgBtgFYBoA3QKgC0AVqflj92Ph3/fD58/1r+vIt2FstQG3irzCCmS+BR8L3foTQFYEtgWSEMAxgJKCOEtz8FhgDM3MlsWVAZQFz0fDqmyQTTQWirlhCoWWF4EPK+gz1ALCbPtACWagOznDynRuGtEJbGMmSQZLAL9AayZsgDRhgAyX4ZsgLSc12TU+B2IQ/j3qVikgGt/jNtcczN9Jx7chmEMu//rV52FO69CWEAZmvZqxe4AbAsA4/avwndBXq01lAtRuHUAB6dJhZZFtoEZ58hepwurgBGDUcFLyU8BiODlwAAa2UQHbwUAKA3ACJ4KUkJUMFLCR6KksFLcAbQwUsBALac/cQWmaIHSNwK7QrgGd4ep8kASgWANkArzTL0kk3wGZRiFyCfIru8F7injP8jDAWQUdUDaAO0CgBtgFYBoA3QKgC0AVoFgDZA6wqgD573w2OjHQAAAABJRU5ErkJggg==");
            //string result = signature.SignUp("QwertyUiop", "qwerty12", "a665a45920422f9d417e4867efdc4fb8a04a1f3fff1fa07e998e86f7f7a27ae3", null);
            Assert.AreEqual("created", result);
        }

        [TestMethod]
        public void TestSignIn()
        {
            classSignature signature = new classSignature();
            publicUserData result = signature.SignIn("user", "a665a45920422f9d417e4867efdc4fb8a04a1f3fff1fa07e998e86f7f7a27ae3");
            //string result = signature.SignUp("QwertyUiop", "qwerty12", "a665a45920422f9d417e4867efdc4fb8a04a1f3fff1fa07e998e86f7f7a27ae3", null);
            Console.WriteLine(result.public_name);
            Assert.AreEqual(1, result.id);
        }



        [TestMethod]
        public void TestUpdateKey()
        {
            classRSA rsa = new classRSA();
            string _puplicBase = rsa.GetPublicBase64();
            classSignature signature = new classSignature();
            string result = signature.SignInUpdateKeys("qqq", "a665a45920422f9d417e4867efdc4fb8a04a1f3fff1fa07e998e86f7f7a27ae3", _puplicBase);
            Console.WriteLine(result);

            Assert.AreEqual("28", result);
        }
    }
}
