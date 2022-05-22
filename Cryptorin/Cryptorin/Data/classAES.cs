using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Cryptorin.Data
{
    public class classAES
    {
        string pass;
        byte[] key;
        Aes aes;
        /// <summary>
        /// Create AES instance
        /// </summary>
        /// <param name="_pass">Get key: 128bit(16 characters), 192bit(24 characters), 256bit(32 characters)</param>
        public classAES(string _pass)
        {
            pass = _pass;

            key = Encoding.ASCII.GetBytes(pass);

            aes = Aes.Create();

            switch (_pass.Length)
            {
                case 16: aes.KeySize = 128; break;
                case 24: aes.KeySize = 192; break;
                case 32: aes.KeySize = 256; break;
                default: break;
            }

            aes.Key = key;
        }


        /// <summary>
        /// Change the password for the created AES instance
        /// </summary>
        /// <param name="_newPassword">Get key: 128bit(16 characters), 192bit(24 characters), 256bit(32 characters)</param>
        public void ChangeAESkey(string _newPassword)
        {
            pass = _newPassword;
            key = Encoding.ASCII.GetBytes(pass);
            aes = Aes.Create();

            switch (_newPassword.Length)
            {
                case 16: aes.KeySize = 128; break;
                case 24: aes.KeySize = 192; break;
                case 32: aes.KeySize = 256; break;
                default: break;
            }
            aes.Key = key;
        }


        /// <summary>
        /// Encrypt message
        /// </summary>
        /// <param name="_message"></param>
        /// <returns></returns>
        public string Encrypt(string _message)
        {
            try
            {
                //зашифрованные данные в виде массива байтов
                byte[] cryptedMessageByteArray;
                using (MemoryStream fileStream = new MemoryStream())
                {
                    byte[] iv = aes.IV;
                    fileStream.Write(iv, 0, iv.Length);
                    using (CryptoStream cryptoStream = new CryptoStream(
                        fileStream,
                        aes.CreateEncryptor(),
                        CryptoStreamMode.Write))
                    {
                        using (StreamWriter encryptWriter = new StreamWriter(cryptoStream))
                        {
                            encryptWriter.WriteLine(_message);
                        }
                    }
                    cryptedMessageByteArray = fileStream.ToArray();
                }
                //конвертируем полученный массив в удобный для чтения и ДАЛЬНЕЙШЕЙ передачи формат
                string base64_cryptedData = Convert.ToBase64String(cryptedMessageByteArray);
                return base64_cryptedData;
            }
            catch (Exception)
            {
                return null;
            }
            
        }



        /// <summary>
        /// Decrypt message
        /// </summary>
        /// <param name="_base64cipher"></param>
        /// <returns></returns>
        public string Decrypt(string _base64cipher)
        {
            try
            {
                byte[] base64EncodedBytes = Convert.FromBase64String(_base64cipher);
                using (MemoryStream fileStream = new MemoryStream(base64EncodedBytes))
                {
                    byte[] iv = new byte[aes.IV.Length];
                    int numBytesToRead = aes.IV.Length;
                    int numBytesRead = 0;
                    while (numBytesToRead > 0)
                    {
                        int n = fileStream.Read(iv, numBytesRead, numBytesToRead);
                        if (n == 0) break;

                        numBytesRead += n;
                        numBytesToRead -= n;
                    }

                    using (CryptoStream cryptoStream = new CryptoStream(
                       fileStream,
                       aes.CreateDecryptor(key, iv),
                       CryptoStreamMode.Read))
                    {
                        using (StreamReader decryptReader = new StreamReader(cryptoStream))
                        {
                            string decryptedMessage = decryptReader.ReadToEnd();
                            return decryptedMessage;
                        }
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }
            

        }
    }
}
