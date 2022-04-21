using Konscious.Security.Cryptography;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cryptorin.Data
{
    public class Argon
    {
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

        public string Argon2id(string _message,string _salt)
        {
            byte[] salt = Encoding.ASCII.GetBytes(_salt);
            byte[] password = Encoding.ASCII.GetBytes(_message);
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
            return hex.ToString();
        }
    }
}
