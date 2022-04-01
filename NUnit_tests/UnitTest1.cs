using NUnit.Framework;
using Cryptorin.Data;

namespace NUnit_tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void AES_test()
        {
            classAES aes = new classAES("4elpsGky8'}|;I[*11111111");
            string crypt = aes.Encrypt("hello");
            string decrypt = aes.Decrypt(crypt);
            Assert.AreEqual("hello",decrypt);
        }
    }
}