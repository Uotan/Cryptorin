using Konscious.Security.Cryptography;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cryptorin.Data
{
    /// <summary>
    /// A class for working with hash functions Argon
    /// </summary>
    public class Argon
    {
        /// <summary>
        /// Hashing with Argon2d
        /// </summary>
        /// <param name="_message">Your string</param>
        /// <param name="_salt">Salt</param>
        /// <returns>Hash</returns>
        public string Argon2d(string _message, string _salt)
        {
            byte[] salt = Encoding.ASCII.GetBytes(_salt);
            byte[] password = Encoding.ASCII.GetBytes(_message);
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
            return hex.ToString();
        }


        /// <summary>
        /// Hashing with Argon2i
        /// </summary>
        /// <param name="_message">Your string</param>
        /// <param name="_salt">Salt</param>
        /// <returns></returns>
        public string Argon2i(string _message, string _salt)
        {
            byte[] salt = Encoding.ASCII.GetBytes(_salt);
            byte[] password = Encoding.ASCII.GetBytes(_message);
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
            return hex.ToString();
        }


        /// <summary>
        /// Hashing with Argon2id
        /// </summary>
        /// <param name="_message">Your string</param>
        /// <param name="_salt">Salt</param>
        /// <returns></returns>
        public string Argon2id(string _message,string _salt)
        {
            byte[] salt = Encoding.ASCII.GetBytes(_salt);
            byte[] password = Encoding.ASCII.GetBytes(_message);
            var argon2id = new Argon2id(password);

            argon2id.DegreeOfParallelism = 2;
            //argon2id.MemorySize = 8192;
            argon2id.MemorySize = 2048;
            argon2id.Iterations = 5;
            argon2id.Salt = salt;

            var hash = argon2id.GetBytes(50);

            StringBuilder hex = new StringBuilder(hash.Length * 2);
            foreach (byte b in hash)
            {
                hex.AppendFormat("{0:x2}", b);
            }
            return hex.ToString();
        }
    }
}
