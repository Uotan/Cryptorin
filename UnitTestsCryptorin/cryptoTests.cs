using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Cryptorin.Data;
using Cryptorin.Classes.SQLiteClasses;
using Cryptorin.Views;
using Cryptorin.Common;
using System.Net;
using System.Collections.Generic;
using Konscious.Security.Cryptography;
using System.Text;
using Cryptorin.Classes;

namespace UnitTestsCryptorin
{
    [TestClass]
    public class cryptoTests
    {
        /// <summary>
        /// Тестирование AES шифрования
        /// </summary>
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



        /// <summary>
        /// Тестирование своих методов RSA (не умеет шифровать более) 
        /// </summary>
        [TestMethod]
        public void TestRSAmethod()
        {
            classRSA rsa = new classRSA();
            string _privateBase = rsa.GetPrivateBase64();
            string _puplicBase = rsa.GetPublicBase64();
            string MyString = "В качестве предметной области, был выбран простой чат 1234";
            string crypt = rsa.Encrypt(MyString, _puplicBase);
           
            var bytesCount1 = ASCIIEncoding.Unicode.GetByteCount(MyString);
            var bytesCount2 = ASCIIEncoding.ASCII.GetByteCount(MyString);

            Console.WriteLine(MyString.Length.ToString());
            Console.WriteLine(bytesCount1);
            Console.WriteLine(bytesCount2);

            string decrypt = rsa.Decrypt(crypt,_privateBase);
            decrypt = decrypt.Trim();

            Console.WriteLine(decrypt);
            //Assert.AreEqual("hello", decrypt);
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
        public void TestGetCountMessage()
        {

            classMessages signature = new classMessages();
            int result = signature.GetCountOfMessagesWithUser(41, 38, "qqq", "1ac673df357fbdaae0fa9c0ff7c1d52ab5a6ca24867dfcdbd5d7ceac2bbefc27b725289a304e3005819cb61ce8514372ed249a59885831c5774095264843b84d02c30233af0c4b258f2d78c70aeb124e511274297575624d70888b9f31bbe3625ff17c3b");

            Console.WriteLine("Count:" + result);
            //Assert.AreEqual(11, result);
        }


        //[TestMethod]
        //public void TestSendMessage()
        //{
        //    classMessages signature = new classMessages();
        //    string result = signature.SendMessage(38, 39, "qqq", "1ac673df357fbdaae0fa9c0ff7c1d52ab5a6ca24867dfcdbd5d7ceac2bbefc27b725289a304e3005819cb61ce8514372ed249a59885831c5774095264843b84d02c30233af0c4b258f2d78c70aeb124e511274297575624d70888b9f31bbe3625ff17c3b", "unit_test");

        //    Console.WriteLine(result);

        //}


        [TestMethod]
        public void TestGetMessages()
        {
            classMessages signature = new classMessages();

            List<fetchedMessage> result = signature.GetMessagesFromUser(38, 41, "qqq", "1ac673df357fbdaae0fa9c0ff7c1d52ab5a6ca24867dfcdbd5d7ceac2bbefc27b725289a304e3005819cb61ce8514372ed249a59885831c5774095264843b84d02c30233af0c4b258f2d78c70aeb124e511274297575624d70888b9f31bbe3625ff17c3b", 1);
            foreach (var item in result)
            {
                Console.WriteLine(item.from_whom + " - " + item.for_whom + ": " + item.rsa_cipher);
            }

        }



        [TestMethod]
        public void TestArgon2d()
        {
            byte[] salt = Encoding.ASCII.GetBytes("saltsalt69");
            //byte[] userUuidBytes;
            byte[] password = Encoding.ASCII.GetBytes("hello");
            var argon2d = new Argon2d(password);

            argon2d.DegreeOfParallelism = 10;
            argon2d.MemorySize = 8192;
            argon2d.Iterations = 20;
            argon2d.Salt = salt;

            var hash = argon2d.GetBytes(100);

            StringBuilder hex = new StringBuilder(hash.Length * 2);
            foreach (byte b in hash)
            {
                hex.AppendFormat("{0:x2}", b);
            }


            Console.WriteLine(hex);
        }


        [TestMethod]
        public void TestArgon2i()
        {
            byte[] salt = Encoding.ASCII.GetBytes("saltsalt69");
            byte[] password = Encoding.ASCII.GetBytes("hello");
            var argon2i = new Argon2i(password);

            argon2i.DegreeOfParallelism = 10;
            argon2i.MemorySize = 8192;
            argon2i.Iterations = 20;
            argon2i.Salt = salt;

            var hash = argon2i.GetBytes(100);

            StringBuilder hex = new StringBuilder(hash.Length * 2);
            foreach (byte b in hash)
            {
                hex.AppendFormat("{0:x2}", b);
            }


            Console.WriteLine(hex);
        }



        [TestMethod]
        public void TestArgon2id()
        {
            byte[] salt = Encoding.ASCII.GetBytes("saltsalt69");
            byte[] password = Encoding.ASCII.GetBytes("hello");
            var argon2id = new Argon2id(password);

            argon2id.DegreeOfParallelism = 10;
            argon2id.MemorySize = 8192;
            argon2id.Iterations = 20;
            argon2id.Salt = salt;

            var hash = argon2id.GetBytes(100);

            StringBuilder hex = new StringBuilder(hash.Length * 2);
            foreach (byte b in hash)
            {
                hex.AppendFormat("{0:x2}", b);
            }


            Console.WriteLine(hex);
        }



        [TestMethod]
        public void TestTrueRSA()
        {
            RSAUtil rSAUtil = new RSAUtil();
            var keys = rSAUtil.CreateKeys();

            string cipher = rSAUtil.Encrypt(keys[1],"hello");
            string decyptedMessage = rSAUtil.Decrypt(keys[0],cipher);
            Console.Write(decyptedMessage);
        }


        [TestMethod]
        public void TestEncyptKey()
        {
            RSAUtil rSAUtil = new RSAUtil();
            List<string> keys = rSAUtil.CreateKeys();
            classAES aES = new classAES(keyClass.AESkey);
            string symmetricallyEncryptedKey = aES.Encrypt(keys[0]);
            Console.Write(symmetricallyEncryptedKey);

        }
}
}
