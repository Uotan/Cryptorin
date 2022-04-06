using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Collections.Specialized;
using Newtonsoft.Json;
using Cryptorin.Classes;

namespace Cryptorin.Data
{
    public class classSignature
    {
        /// <summary>
        /// Checking for the existence of a login
        /// </summary>
        /// <param name="_login"></param>
        /// <returns></returns>
        public string CheckLoginExists(string _login)
        {
            WebClient client = new WebClient();
            NameValueCollection param = new NameValueCollection();
            param.Add("login", _login);
            try
            {
                var response = client.UploadValues("https://cryptorin.ru/API/checkLoginCoincidence.php", "POST", param);
                string result = Encoding.Default.GetString(response);
                result = result.Trim();
                return result;
            }
            catch (Exception)
            {
                return null;
            }

        }





        /// <summary>
        /// Registration method
        /// </summary>
        /// <param name="_publicName"></param>
        /// <param name="_login"></param>
        /// <param name="_pass"></param>
        /// <param name="_imageBade64"></param>
        /// <returns></returns>
        public string SignUp(string _publicName, string _login, string _pass, string _imageBade64)
        {
            WebClient client = new WebClient();
            NameValueCollection param = new NameValueCollection();
            param.Add("publicName", _publicName);
            param.Add("login", _login);
            param.Add("password", _pass);
            param.Add("image", _imageBade64);
            try
            {
                var response = client.UploadValues("https://cryptorin.ru/API/signup.php", "POST", param);
                string result = Encoding.Default.GetString(response);
                result = result.Trim();
                return result;
            }
            catch (Exception)
            {
                return null;
            }

        }






        /// <summary>
        /// Sign In method
        /// </summary>
        /// <param name="_login"></param>
        /// <param name="_pass"></param>
        /// <returns></returns>
        public publicUserData SignIn(string _login, string _pass)
        {
            WebClient client = new WebClient();
            NameValueCollection param = new NameValueCollection();
            param.Add("login", _login);
            param.Add("password", _pass);
            try
            {
                var response = client.UploadValues("https://cryptorin.ru/API/signin.php", "POST", param);
                string result = Encoding.Default.GetString(response);
                publicUserData userData = JsonConvert.DeserializeObject<publicUserData>(result);
                return userData;
            }
            catch (Exception)
            {
                return null;
            }

        }


        /// <summary>
        /// Update RSA public key in a Data Base
        /// </summary>
        /// <param name="_login"></param>
        /// <param name="_pass"></param>
        /// <param name="_publicKey"></param>
        /// <returns>return the current key number in the database</returns>
        public string SignInUpdateKeys(string _login, string _pass, string _publicKey)
        {
            WebClient client = new WebClient();
            NameValueCollection param = new NameValueCollection();
            param.Add("login", _login);
            param.Add("password", _pass);
            param.Add("publicKey", _publicKey);
            try
            {
                var response = client.UploadValues("https://cryptorin.ru/API/newRSAkey.php", "POST", param);
                string stringResult = Encoding.Default.GetString(response);
                stringResult = stringResult.Trim();
                //int currentKeyNumber = Convert.ToInt32(stringResult);
                //return currentKeyNumber;
                return stringResult;
            }
            catch (Exception)
            {
                //according to the logic of the work, the key number cannot be zero
                //therefore, in case of an error, the function will return 0
                return null;
                //return 0;
            }

        }


        public string GetImage(int _id)
        {
            WebClient client = new WebClient();
            NameValueCollection param = new NameValueCollection();
            param.Add("id", _id.ToString());
            try
            {
                var response = client.UploadValues("https://cryptorin.ru/API/getbase64.php", "POST", param);
                string resultBase64 = Encoding.Default.GetString(response);
                resultBase64 = resultBase64.Trim();
                return resultBase64;
            }
            catch (Exception)
            {
                return null;
            }

        }
    }
}
