using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Cryptorin.Data
{

    public class classSHA256
    {
        /// <summary>
        /// get hash - SHA256
        /// </summary>
        /// <param name="rawData"></param>
        /// <returns></returns>
        public string ComputeSha256Hash(string rawData)
        {
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                if (rawData==null)
                {
                    return null;
                }
                else
                {
                    // ComputeHash - returns byte array  
                    byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                    // Convert byte array to a string   
                    StringBuilder builder = new StringBuilder();
                    for (int i = 0; i < bytes.Length; i++)
                    {
                        builder.Append(bytes[i].ToString("x2"));
                    }
                    return builder.ToString();
                }
                
            }
        }
    }
}
