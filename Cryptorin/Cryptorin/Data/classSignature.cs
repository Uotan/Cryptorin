using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Collections.Specialized;

namespace Cryptorin.Data
{
    public class classSignature
    {
        public string CheckLoginExists(string _login)
        {
            WebClient client = new WebClient();
            NameValueCollection param = new NameValueCollection();
            param.Add("login", _login);
            var response = client.UploadValues("https://cryptorin.ru/API/checkLoginCoincidence.php", "POST", param);
            string result = Encoding.Default.GetString(response);
            result = result.Trim();
            return result;
        }
        /// <summary>
        /// Registration method
        /// </summary>
        /// <param name="_publicName"></param>
        /// <param name="_login"></param>
        /// <param name="_pass"></param>
        /// <param name="_imageBade64"></param>
        /// <returns></returns>
        public string SignUp(string _publicName, string _login,string _pass, string _imageBade64)
        {
            WebClient client = new WebClient();
            NameValueCollection param = new NameValueCollection();
            param.Add("publicName", _publicName);
            param.Add("login", _login);
            param.Add("password", _pass);
            param.Add("image", _imageBade64);
            var response = client.UploadValues("https://cryptorin.ru/API/signup.php", "POST", param);
            string result = Encoding.Default.GetString(response);
            result = result.Trim();
            return result;
        }
        //public string SignIn(string _login,string _pass)
        //{
        //    WebClient client = new WebClient();
        //    NameValueCollection param = new NameValueCollection();
        //    param.Add("login", _login);
        //    param.Add("password", _pass);
        //    var response = client.UploadValues("https://cryptorin.ru/API/signin.php", "POST", param);
        //    string result = Encoding.Default.GetString(response);
        //    result = result.Trim();
        //    return result;
        //}
    }
}
