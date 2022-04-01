using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Serialization;


namespace Cryptorin.Data
{
    public class classRSA
    {
        byte[] publicKeyBytes;
        byte[] privateKeyBytes;


        RSACryptoServiceProvider cspNew;
        RSACryptoServiceProvider csp;
        RSAParameters _privatekey;
        RSAParameters _publickey;

        public classRSA()
        {
            csp = new RSACryptoServiceProvider();
            RSAexportKeys();
            publicKeyBytes = GetPublicKey();
            privateKeyBytes = GetPrivateKey();
        }

        public void RSAexportKeys()
        {
            SetKeySize();
            _privatekey = csp.ExportParameters(true);
            _publickey = csp.ExportParameters(false);
        }

        public void SetKeySize()
        {
            csp.KeySize = 2048;
        }

        public byte[] GetPublicKey()
        {
            var sw = new StringWriter();
            XmlSerializer xs = new XmlSerializer(typeof(RSAParameters));
            xs.Serialize(sw, _publickey);
            byte[] key = Encoding.ASCII.GetBytes(sw.ToString());
            return key;

        }

        public byte[] GetPrivateKey()
        {
            var sw = new StringWriter();
            XmlSerializer xs = new XmlSerializer(typeof(RSAParameters));
            xs.Serialize(sw, _privatekey);
            byte[] key = Encoding.ASCII.GetBytes(sw.ToString());
            return key;
        }

        public string Encrypt(string plainText)
        {
            cspNew = new RSACryptoServiceProvider();
            cspNew.ImportParameters(_publickey);
            var data = Encoding.Unicode.GetBytes(plainText);
            var cypher = csp.Encrypt(data, false);
            return Convert.ToBase64String(cypher);
        }

        public string Decrypt(string cypherText)
        {
            var dataBytes = Convert.FromBase64String(cypherText);
            cspNew.ImportParameters(_privatekey);
            var plainText = csp.Decrypt(dataBytes, false);
            return Encoding.Unicode.GetString(plainText);
        }

    }
}
